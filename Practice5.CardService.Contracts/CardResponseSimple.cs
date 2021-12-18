namespace Practice5.CardService.Contracts
{
    public record CardResponseSimple(string SafePan, decimal Balance);
    public record CardResponseSimpleV2(string SafePan1, decimal Balance);
}