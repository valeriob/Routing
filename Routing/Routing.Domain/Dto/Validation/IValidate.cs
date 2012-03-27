using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Routing.Domain.Dto.Validation
{
    public interface IValidatable
    {
        ValidationResult Validate();
    }

    //public class Always_Right : IValidatable
    //{
    //    public ValidationResult Validate()
    //    {
    //        return new ValidationResult(this);
    //    }
    //}

    public class ValidationResult
    {
        public IValidatable Source { get; protected set; }
        public bool IsValid { get; protected set; }
        public IEnumerable<ValidationItem> Items { get; protected set; }
        protected List<ValidationItem> _Items;

        public ValidationResult(IValidatable source)
        {
            Source = source;
            IsValid = true;
            _Items = new List<ValidationItem>();
        }

        public void Throw_If_Is_Not_Valid()
        {
            if (!IsValid)
                throw new ValidationException(Items.Select(v=> v.Description) , Source.GetType() );
        }

        public void Append(ValidationType type, string description)
        {
            _Items.Add(new ValidationItem { Type = type, Description = description });
            if (type == ValidationType.Error)
                IsValid = false;
        }
    }
    public class ValidationItem
    {
        public ValidationType Type { get; set; }
        public string Description { get; set; }
    }

    public enum ValidationType {Info, Warning, Error }


    public class ValidationException : Exception
    {
        public IEnumerable<string> Errors { get; protected set; }
        public Type Source { get; protected set; }

        public ValidationException(IEnumerable<string> errors, Type source)
        {
            Errors = errors;
            Source = source;
        }

        public override string ToString()
        {
            return string.Format("Error Validating {0}", Source.Name);
        }
    }

    public static class Extensions
    {
        public static void Throw_If_Is_Not_Valid(this IValidatable to_be_validated)
        {
            var result = to_be_validated.Validate();
            result.Throw_If_Is_Not_Valid();
        }
    }

  

}
