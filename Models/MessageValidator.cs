using System;
using System.ComponentModel.DataAnnotations;

namespace CoderHelp {

    public class MessageValidator {
        [Required(ErrorMessage = "The message field is required!")]
        [MinLength(4)]
        public string Message { get; set; }

        [Required(ErrorMessage = "The description field is required!")]
        [MinLength(10)]
        public string Description { get; set; }
        
        public string CodeSnippit { get; set; }

        public string Bug { get; set; }

    }




}