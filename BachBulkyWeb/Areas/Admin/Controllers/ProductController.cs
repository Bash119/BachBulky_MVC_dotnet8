using Bulky.DataAccess.Data.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BachBulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products= _unitOfWork.Product.GetAll().ToList();
          
            return View(products);
        }

        public IActionResult Upsert(int? id) 
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()

            };
           
            // ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"]  = CategoryList;
        
            if(id == null || id ==0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
           
        }

        [HttpPost]
        public IActionResult Upsert( ProductVM productVM,IFormFile? file)
        {
            //if(product.Product.Title == product.Product.Description)
            //{
            //    ModelState.AddModelError("name","The Description cannot match the Title.");
            //}
            if(ModelState.IsValid)
            {
                string wwwRootPath= _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName=Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath= Path.Combine(wwwRootPath, @"images\product");

                    using (var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["Success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                productVM.CategoryList=_unitOfWork.Category.GetAll().Select(u=> new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
           
           
            
        }

        //public IActionResult Edit(int? id)
        //{
        //    if(id == 0 || id == null )return NotFound();
        //    Product obj = _unitOfWork.Product.Get(u=>u.Id == id);
        //    if(obj == null) return NotFound();
        //    return View(obj);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Product edited successfully";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    return View(obj);
        //}

        public IActionResult Delete(int? id)
        {
            if(id ==0 || id == null) return NotFound();
            Product product = _unitOfWork.Product.Get(u => u.Id == id);
            if(product == null) return NotFound();
            return View(product);

        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if(obj == null) return NotFound();
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product deleted successfully";
            return RedirectToAction("Index", "Product");

        }


    }
}
