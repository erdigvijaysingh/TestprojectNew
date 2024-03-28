using consumeApiRoleBased.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;



namespace consumeApiRoleBased.Controllers
{
    
    public class AuthController : Controller
    {
        
        private readonly HttpClient _httpClient;
        //public string baseurl = "https://localhost:44335/api/Authenticate/register";
        
        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            // Serialize the form data to JSON
            string jsonData = JsonConvert.SerializeObject(model);

            using (HttpClient client = new HttpClient())
            {
                // Create StringContent with JSON data
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send HTTP POST request to API endpoint
                HttpResponseMessage response = await client.PostAsync("https://localhost:44335/api/Authenticate/register", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Handle success response here
                    return Ok(new Response { Status="success",Message="User Registred successfully"});

                }
                else
                {
                    // Handle error response here
                }
            }

            // Return view or redirect
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            // Serialize the form data to JSON
            string jsonData = JsonConvert.SerializeObject(model);

            using (HttpClient client = new HttpClient())
            {
              
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

               
                HttpResponseMessage response = await client.PostAsync("https://localhost:44335/api/Authenticate/Login", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //string token = responseBody;
                    string json = responseBody.Trim('.');
                    // Parse the JSON string
                    JObject jsonObject = JObject.Parse(json);

                    // Access the value of the "token" key
                    //string token = (string)jsonObject["token"];
                    HttpContext.Session.SetString("Acesstoken", json);


                    return Ok(json);
                }
                else
                {
                    // Handle error response here
                }
            }

            // Return view or redirect
            return RedirectToAction("getemployee");
        }

        public async Task<IActionResult> employee()
        {
            //var product = await getemployee();
            return View();
        }
        public async Task<List<Employee>> GetEmployee()
        {
            // Retrieve access token from the request header
            string token = HttpContext.Request.Headers["Authorization"];

            // Check if token is not null and starts with "Bearer "
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                // Remove "Bearer " prefix from the token
                token = token.Substring("Bearer ".Length);
            }

            using (HttpClient client = new HttpClient())
            {
                // Set the authorization header with the retrieved token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync("https://localhost:44335/api/Employees/GetEmployee");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to List<Employee>
                    string jsonContent = await response.Content.ReadAsStringAsync();
                    List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(jsonContent);

                    return employees;
                }
                else
                {
                    // Handle error response here
                    return new List<Employee>();
                }
            }
        }

        public IActionResult addemploye()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EmployeeAdd(Employee model)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                // Remove "Bearer " prefix from the token
                token = token.Substring("Bearer ".Length);
            }
            string jsonbody= JsonConvert.SerializeObject(model);
            
            // Check if token is not null and starts with "Bearer "
           
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpContent content= new StringContent(jsonbody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("https://localhost:44335/api/Employees/empadd",content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }

        }

        public async Task<IActionResult>Edit(int id)
        {
            var emp = await getempbyid(id);
            return View(emp);
        }
        [HttpGet]
        public async Task<IActionResult> getempbyid(int id)
        {
            string token = HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
            {
                // Remove "Bearer " prefix from the token
                token = token.Substring("Bearer ".Length);

                // Define the base URL of the API
                string baseUrl = "https://localhost:44335/api/";

                // Specify the endpoint with the employee ID as a parameter
                string endpoint = $"getbyId/{id}";

                // Set up HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Set Authorization header
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    try
                    {
                        // Make the GET request
                        HttpResponseMessage response = await client.GetAsync(baseUrl + endpoint);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Read the response content
                            string responseBody = await response.Content.ReadAsStringAsync();

                            // You can process the responseBody as per your requirements
                            // For example, you can deserialize it if it's JSON
                             var employee = JsonConvert.DeserializeObject<Employee>(responseBody);

                            // Return the response
                            return Ok(employee);
                        }
                        else
                        {
                            // Return an appropriate error response
                            return StatusCode((int)response.StatusCode, "Failed to get employee data");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        // Handle exceptions
                        return StatusCode(500, $"Internal Server Error: {ex.Message}");
                    }
                }
            }
            else
            {
                // Return Unauthorized if token is missing or invalid
                return Unauthorized();
            }
        }


    }
}
