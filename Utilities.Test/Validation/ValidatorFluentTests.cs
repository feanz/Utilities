using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareApproach.TestingExtensions;
using Utilities.Validation;

namespace Utilities.Test.Validation
{
    [TestClass]
    public class ValidatorFluentTests
    {
        [TestMethod]
        public void FluentValidator_Should_Return_No_Errors_For_A_Valid_Entity()
        {
            var val = new FluentValidator();
            var user = ValideUser;

            val.Validate(() => user.UserName).IsNotNull()
                .Validate(() => user.CreateDate).IsBeforeToday()
                .Validate(() => user.Email).IsValidEmail()
                .Validate(() => user.MobilePhone).IsValidMobileNumber()
                .Validate(() => user.FailedLogonAttemp).IsBetween(0, 5);

            val.HasErrors.ShouldBeFalse();
            val.Errors.Count.ShouldEqual(0);
        }

        [TestMethod]
        public void FluentValidator_Should_Return_Errors_For_Null_Property_Validation_Check()
        {
            var val = new FluentValidator();
            var user = ValideUser;

            user.UserName = null;

            val.Validate(() => user.UserName).IsNotNull();

            val.HasErrors.ShouldBeTrue();
            val.Errors.Count.ShouldEqual(1);
        }

        [TestMethod]
        public void FluentValidator_Should_Return_No_Errors_For_Validate_Function_()
        {
            var val = new FluentValidator();
            var user = ValideUser;

            val.Validate(() => user.UserName).Must(o => o.ToString().Length > 3,"User name is to short");
            
            val.HasErrors.ShouldBeFalse();
            val.Errors.Count.ShouldEqual(0);
        }

        [TestMethod]
        public void FluentValidator_Should_Return_Errors_For_invalide_Validate_Function_()
        {
            var val = new FluentValidator();
            var user = ValideUser;

            user.UserName = "St";
            val.Validate(() => user.UserName).Must(o => o.ToString().Length > 3, "User name is to short");

            val.HasErrors.ShouldBeTrue();
            val.Errors.Count.ShouldEqual(1);
        }
        
        /// Add more validation tests

        private static User ValideUser
        {
            get
            {
                var user = new User
                               {
                                   UserName = "Steve",
                                   CreateDate = DateTime.Now.AddDays(-5),
                                   Email = "example@email.com",
                                   MobilePhone = "07798937597",
                                   FailedLogonAttemp = 1
                               };
                return user;
            }
        }
    }

    public class User
    {
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public int FailedLogonAttemp { get; set; }

        public bool IsValid()
        {
            var val = new FluentValidator();
            val.Validate(() => UserName).IsNotNull()
                .Validate(() => CreateDate).IsAfterToday()
                .Validate(() => Email).IsValidEmail()
                .Validate(() => MobilePhone).IsValidMobileNumber();

            return val.HasErrors;
        }
    }
}
