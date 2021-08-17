using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Vwm.RTree.Api.Attributes
{
  public class LengthDimensionClampedAttribute: ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (!(value is ICollection collection))
        throw new InvalidOperationException($"Invalid usage of '{typeof(LengthDimensionClampedAttribute)}'" +
          $"target must be of type '{typeof(ICollection).Name}'");

      if (collection.Count != Constants._Dimensions)
        return new ValidationResult($"Collection has invalid count of elemnts, " +
          $"Provided '{collection.Count}', expected '{Constants._Dimensions}'");

      return ValidationResult.Success;
    }
  }
}
