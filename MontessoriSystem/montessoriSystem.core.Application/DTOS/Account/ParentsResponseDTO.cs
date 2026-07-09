using MontessoriSystem.Core.Application.Common;
using MontessoriSystem.Core.Application.DTOS.Account;

public class ParentsResponseDTO
{
    public List<ClassSelected<string>> Fathers { get; set; } = new();
    public List<ClassSelected<string>> Mothers { get; set; } = new();
}
