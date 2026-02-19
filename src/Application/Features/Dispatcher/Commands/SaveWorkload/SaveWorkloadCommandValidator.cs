using Domain.Abstractions.Result;
using FluentValidation;

namespace Application.Features.Dispatcher.SaveWorkload;

public class SaveWorkloadCommandValidator : AbstractValidator<SaveWorkloadCommand>
{
    public SaveWorkloadCommandValidator()
    {
        RuleFor(x => x.WorkloadSummary)
            .NotEmpty().WithState(_ => ErrorCode.InvalidWorkloadSummary)
            .WithMessage("Поле {PropertyName} не может быть пустым")
            .DependentRules(() =>
            {
                RuleForEach(x => x.WorkloadSummary.WorkloadList)
                    .NotEmpty().WithState(_ => ErrorCode.InvalidWorkloadSummary)
                    .WithMessage("Поле {PropertyName} не может быть пустым")
                    .ChildRules(workLoadList =>
                    {
                        workLoadList.RuleFor(x => x.LessonName)
                            .NotEmpty().WithState(_ => ErrorCode.InvalidWorkload)
                            .WithMessage("Поле {PropertyName} не может быть пустым")
                            .MaximumLength(100).WithState(_ => ErrorCode.InvalidWorkload)
                            .WithMessage("Поле {PropertyName} должно быть не больше 100 символов")
                            .MinimumLength(2).WithState(_ => ErrorCode.InvalidWorkload)
                            .WithMessage("Поле {PropertyName} должно быть не менее 2 символов");
                        workLoadList.RuleFor(x => x.LessonIndex)
                            .NotEmpty().WithState(_ => ErrorCode.InvalidWorkload)
                            .WithMessage("Поле {PropertyName} не может быть пустым")
                            .GreaterThan(0).WithState(_ => ErrorCode.InvalidWorkload)
                            .WithMessage("Поле {PropertyName} должно быть больше 0");
                        workLoadList.RuleFor(x => x.LessonDate)
                            .NotEmpty().WithState(_ => ErrorCode.InvalidWorkload)
                            .WithMessage("Поле {PropertyName} не может быть пустым")
                            .Must(x => x >= DateTime.Today).WithState(_ => ErrorCode.InvalidWorkload)
                            .WithMessage("Поле {PropertyName} должно быть больше или равно сегодняшнему дню");
                        
                    });
                RuleForEach(x => x.WorkloadSummary.RemovalList)
                    .NotEmpty().WithState(_ => ErrorCode.InvalidRemoval)
                    .WithMessage("Поле {PropertyName} не может быть пустым")
                    .ChildRules(removalList =>
                    {
                        removalList.RuleFor(x => x.RemovalIndex)
                            .NotEmpty().WithState(_ => ErrorCode.InvalidRemoval)
                            .WithMessage("Поле {PropertyName} не может быть пустым")
                            .GreaterThan(0).WithState(_ => ErrorCode.InvalidRemoval)
                            .WithMessage("Поле {PropertyName} должно быть больше 0");
                        removalList.RuleFor(x => x.FirstLessonIndex)
                            .NotEmpty().WithState(_ => ErrorCode.InvalidRemoval)
                            .WithMessage("Поле {PropertyName} не может быть пустым")
                            .GreaterThan(0).WithState(_ => ErrorCode.InvalidRemoval)
                            .WithMessage("Поле {PropertyName} должно быть больше 0");
                        removalList.RuleFor(x => x.SecondLessonIndex)
                            .NotEmpty().WithState(_ => ErrorCode.InvalidRemoval)
                            .WithMessage("Поле {PropertyName} не может быть пустым")
                            .GreaterThan(0).WithState(_ => ErrorCode.InvalidRemoval)
                            .WithMessage("Поле {PropertyName} должно быть больше 0");
                    });
            });
    }
}
