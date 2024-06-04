using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrganicFoodMVC.DataAccess.Repository.IRepository;
using OrganicFoodMVC.Models;
using OrganicFoodMVC.Models.ViewModels;
using OrganicFoodMVC.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Diagnostics;

namespace OrganicFoodMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        // anh xa du lieu
        [BindProperty]
        public OrderDetailsVM orderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = SD.Role_User_Indi + "," + SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult Index(string status)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        
            IEnumerable<OrderDetails> orderDetails = _unitOfWork.OrderDetails.GetAll(
                                                        u => u.OrderHeader.ApplicationUserId == claim.Value,
                                                        includeProperties: "OrderHeader,Product");
         

            // status order
            switch (status)
            {
                case "pending":
                    orderDetails = orderDetails.Where(o => o.OrderHeader.PaymentStatus == SD.StatusPending);
                    break;
                case "inprocess":
                    orderDetails = orderDetails.Where(o => o.OrderHeader.OrderStatus == SD.StatusApproved ||
                                                            o.OrderHeader.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderDetails = orderDetails.Where(o => o.OrderHeader.OrderStatus == SD.StatusShipped);
                    break;
                case "rejected":
                    orderDetails = orderDetails.Where(o => o.OrderHeader.OrderStatus == SD.StatusCancelled ||
                                                            o.OrderHeader.OrderStatus == SD.StatusRefunded ||
                                                            o.OrderHeader.OrderStatus == SD.PaymentStatusRejected);
                    break;
                default:
                    break;
            }

            IEnumerable<OrderHeader> orderId = _unitOfWork.OrderHeader.GetAll(x => x.ApplicationUserId == claim.Value && x.OrderStatus != SD.StatusCancelled);
            SelectList orderIdList = new SelectList(orderId, "Id", "Id");
            ViewBag.orderIdList = orderIdList;
            return View(orderDetails);
        }

        [Authorize(Roles = SD.Role_User_Indi + "," + SD.Role_Admin + "," + SD.Role_Employee)]
        [HttpGet]
        public IActionResult PrintfDetail(string id)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x=>x.Id == int.Parse(id));
            List<OrderDetails> orderDetails = _unitOfWork.OrderDetails.GetAll(
                                                        u => u.OrderId == int.Parse(id),
                                                        includeProperties: "OrderHeader,Product").ToList();

            string sourceFile = "D:\\datn_cntt_k14\\FreshMarket\\FreshMarketMVC\\wwwroot\\File\\OrderDetailOriginal.pdf";
            string destinationFile = "D:\\datn_cntt_k14\\FreshMarket\\FreshMarketMVC\\wwwroot\\File\\OrderDetail.pdf";
            System.IO.File.Copy(sourceFile, destinationFile, true);

            PdfDocument document = PdfReader.Open(destinationFile, PdfDocumentOpenMode.Modify);
            //PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages[0];
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 9, XFontStyle.Bold);

            //tiêu đề
            gfx.DrawString(id, font, XBrushes.Red, new XRect(150, -238, page.Width, page.Height), XStringFormats.Center);
            gfx.DrawString(DateTime.Now.Date.ToString("dd/MM/yyyy"), font, XBrushes.Black, new XRect(135, -222, page.Width, page.Height), XStringFormats.Center);
            gfx.DrawString("Trạng thái:", font, XBrushes.Black, new XRect(100, -208, page.Width, page.Height), XStringFormats.Center);
            gfx.DrawString(orderHeader.OrderStatus, font, XBrushes.Red, new XRect(440, -208, page.Width, page.Height), XStringFormats.CenterLeft);

            int x1 = 115, x2 = 400, x3 = 425, y = -120;
            foreach (var order in orderDetails)
            {
                //chi tiết
                gfx.DrawString(order.Product.Name, font, XBrushes.Black, new XRect(x1, y, page.Width, page.Height), XStringFormats.CenterLeft);
                gfx.DrawString(order.Count.ToString(), font, XBrushes.Black, new XRect(x2, y, page.Width, page.Height), XStringFormats.CenterLeft);
                gfx.DrawString(order.Price.ToString(), font, XBrushes.Black, new XRect(x3, y, page.Width, page.Height), XStringFormats.CenterLeft);

                y = y + 16;
            }

            //Người mua
            gfx.DrawString(orderHeader.Name, font, XBrushes.Red, new XRect(150, 205, page.Width, page.Height), XStringFormats.CenterLeft);

            //tổng tiền
            gfx.DrawString(orderHeader.OrderTotal.ToString(), font, XBrushes.Red, new XRect(415, 184, page.Width, page.Height), XStringFormats.CenterLeft);
            
            document.Save(destinationFile);
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(destinationFile)
            {
                UseShellExecute = true
            };
            p.Start();

            return Json(new { Response = true });
        }
    }
}
