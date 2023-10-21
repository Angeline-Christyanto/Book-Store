using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBookOrder.Data;
using MyBookOrder.Models;
using MyBookOrder.Models.Request;
using MyBookOrder.Models.Result;

namespace MyBookOrder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly AppDbContext context;

        public OrderController(AppDbContext context)
        {
            this.context = context;
        }

        // GET Order
        [HttpGet]
        /* async: asynchronous. segala sesuatu yg I/O umumnya pake async karna kita kan ambil data dari 
                  database dan kita harus tunggu database itu respons. 
        IEnurable: enurable --> datanya bisa dihitung atau diakses one by one */
        public async Task<ActionResult<IEnumerable<GetOrderResult>>> Get()
        {
            var orders = await context.Order
                .Include(x => x.Book)
                .OrderBy(x => x.OrderId)
                .Select(x => new GetOrderResult()
                {
                    OrderId = x.OrderId,
                    Quantity = x.Quantity,
                    TotalPrice = x.TotalPrice,
                    Book = x.Book
                })
            .ToListAsync();

            var response = new ApiResponse<IEnumerable<GetOrderResult>>
            {
                StatusCode = StatusCodes.Status200OK,
                RequestMethod = HttpContext.Request.Method,
                Data = orders
            };
            return Ok(response);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<GetOrderResult>> Get(string Id)
        {
            var order = await context.Order
                .Include(x => x.Book)
                .Where(x => x.OrderId == Id)
                .Select(x => new GetOrderResult()
                {
                    OrderId = x.OrderId,
                    Quantity = x.Quantity,
                    TotalPrice = x.TotalPrice,
                    Book = x.Book
                })
            .FirstOrDefaultAsync();

            if (order == null) {
                return NotFound("Order Not Found");
            }

            var response = new ApiResponse<GetOrderResult>
            {
                StatusCode = StatusCodes.Status200OK,
                RequestMethod = HttpContext.Request.Method,
                Data = order
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderRequest createOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if order exists by order id
            var checkOrder = context.Order.Where(x => x.OrderId == createOrderRequest.OrderId).Count();

            if (checkOrder > 0)
            {
                return NotFound("Order Already Exists");
            }

            // check if book exists by book id 
            var checkBook = context.Book.Where(x => x.BookId == createOrderRequest.BookId).Count();

            if (checkBook == 0)
            {
                return NotFound("Order Not Found");
            }

            // user input for creating new order
            var order = new Order
            {
                OrderId = createOrderRequest.OrderId,
                Quantity = createOrderRequest.Quantity,
                BookId = createOrderRequest.BookId
            };
            context.Add(order);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id, [FromBody] UpdateOrderRequest updateOrderRequest)
        {
            /* model state: ensure that request data is valid based on any validation rules 
               specified in "createStudentRequest" class. 
               If data not valid, it returns "BadRequest" (validation error) */
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /* "FirstOrDefaultAsync" is used to retrieve the first order from 
              context.Order collection where the "OrderId" matches the "id" provided 
              indo: ambil data yg paling pertama, kalau ga ada datanya return ke default yaitu 0 data */
            var order = await context.Order.FirstOrDefaultAsync(o => o.OrderId == Id);
            if (order == null)
            {
                return NotFound("Order Not Found");
            }

            /* checkbook pake == 0 karena meskipun dia bentuknya string id, disini ada ".Count" yg artinya 
               checkbook ini int (banyak buku) */
            var checkBook = context.Book.Where(x => x.BookId == updateOrderRequest.BookId).Count();
            if (checkBook == 0)
            {
                return NotFound("Book Not Found");
            }

            // Fetch the associated book using the BookId from the request
            var book = await context.Book.FirstOrDefaultAsync(b => b.BookId == updateOrderRequest.BookId);

            if (book == null)
            {
                return NotFound("Book Not Found");
            }

            order.Quantity = updateOrderRequest.Quantity;
            order.BookId = updateOrderRequest.BookId;
            order.TotalPrice = book.Price * order.Quantity; // Calculate the total price

            // Update the order in the database
            context.Order.Update(order);

            /* "await" keyword indicates that this operation should be asynchronous, 
               which is common in database queries to avoid blocking the main thread */
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            /* var order = await gunanya: karna kita connext ke database jd await untuk tunggu sampe 
               databasenya respon atau reply. Lalu kita bisa akses database Order (context), 
               lalu "FirstOrDefaultAsync" untuk cari data pertama, kalo ga ada datanya di return ke default (data = 0) */
            var order = await context.Order.FirstOrDefaultAsync(x => x.OrderId == Id);

            if (order == null)
            {
                return NotFound("Order Not Found");
            }

            context.Order.Remove(order);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
