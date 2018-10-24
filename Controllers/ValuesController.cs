using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sample;

namespace temp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class loginController : ControllerBase
    {

        IUserRepository IUserobj;
        public loginController(IUserRepository UserRepository)
        {
            this.IUserobj = UserRepository;
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get([FromHeader]string token)
        {

            var value = TokenManager.ValidateMyToken(token);

            if (value == null)
            {
                return Unauthorized();
            }

            else
            {
                return Ok("access granted");
            }

        }

        // GET api/values/5
        // [HttpGet("{id}")]
        // public ActionResult<string> Get(int id)
        // {
        //     return "value";
        // }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            User u = IUserobj.GetUser(user.Email);
            if (u == null)
                return BadRequest("The User does not exist.");

            bool credentials = u.Password.Equals(user.Password);
            if (!credentials)
            { return BadRequest("The username/password combination was wrong."); }

            else
            {
                var token = TokenManager.GenerateToken(user.Email);
                var pass = IUserobj.Hash(user.Password);
                Console.WriteLine("dady!!!!!!!!!!!!!!!!!" + pass);

                return Ok(token);

            }

        }

        [HttpPost()]

        [Route("/sme/signIn")]

        public string smeSignIn([FromBody] signIn signIn)
        {
            if (ModelState.IsValid)
            {
                User u = IUserobj.GetUser(signIn.email);
                if (u == null)
                {
                    string x="user does not exist";
                    return JsonConvert.SerializeObject(x);
                }

                if(!(u.Password.Equals(signIn.password)))
                {
                    string x = "User Password combination is not correct";
                    return JsonConvert.SerializeObject(x) ;
                }

                if(!(u.Designation.Equals("SME")))
                {
                    string x = "You are a learner, Please sign in through learner page";
                    return JsonConvert.SerializeObject(x) ;

                }    

                bool credentials = (u.Password.Equals(signIn.password) && (u.Designation.Equals("SME")));
                if (credentials)
                { 
                    
                    string token = TokenManager.GenerateToken(signIn.email);
                    var pass = IUserobj.Hash(signIn.password);
                    Console.WriteLine("dady!!!!!!!!!!!!!!!!!" + pass);

                    return token.ToString();

                }

        
            }
            return "invalid model state";
        }


        [HttpPost()]

        [Route("/sme/signUp")]

        public string smeSignUp([FromBody]signUp signUp)
        {
            User u = IUserobj.GetUser(signUp.email);
            
           // var s = IUserobj.Hash(signUp.fullName);
            //var secureKey = s.Substring(0, 5);
            string secureKey="mybro";
            if(u != null)
            {
                string x = "User already exist";
                return JsonConvert.SerializeObject(x) ;
            }
            if(signUp.securityKey != secureKey)
            {
                string x = "Wrong Security Key entered";
                return JsonConvert.SerializeObject(x) ;

            }
            bool confirm = ((u == null) && (signUp.securityKey == secureKey));
            if (confirm)
            {
               User k=new User();
               //new User();
                k.Email = signUp.email;
                k.Password= signUp.password;
               // u.Password = IUserobj.Hash(signUp.password);
                k.FullName = signUp.fullName;
                k.Designation = "SME";
                // Console.WriteLine(k);
                IUserobj.Register(k);
                string x = "Success";
                return JsonConvert.SerializeObject(x) ;

            }
            else
            {
                string x = "some other case";
                return JsonConvert.SerializeObject(x) ;
            }



        }




        [HttpPost]

        [Route("/learner/signIn")]

        public string learnerSignIn()
        {
            return "mybro";
        }


        [HttpPost]

        [Route("/learner/signUp")]

        public string learnerSignUp(dynamic SignUp)
        {
            return "mybro";
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
