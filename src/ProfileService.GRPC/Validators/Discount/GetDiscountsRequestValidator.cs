﻿using FluentValidation;
using ProfileService.GRPC.ValidationRules;

namespace ProfileService.GRPC.Validators.Discount
{
    /// <summary>
    /// Validation rules for <seealso cref="GetDiscountsRequest"/>
    /// </summary>
    public class GetDiscountsRequestValidator : AbstractValidator<GetDiscountsRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDiscountsRequestValidator"/> class.
        /// </summary>
        public GetDiscountsRequestValidator()
        {
            RuleFor(e => e.Profilebyidrequest.Id)
                .MustBeValidGuid();
        }
    }
}
