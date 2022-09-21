namespace ties4560_demo3.Controllers
{
  internal class ErrorMessage
  {
    internal ErrorMessage () { }

    internal ErrorMessage(string Reason)
    {
      this.Reason = Reason;
    }

    public string Reason { get; set; }
  }
}