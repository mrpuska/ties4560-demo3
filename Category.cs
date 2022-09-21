using System;

namespace ties4560_demo3
{
  public class Category
  {
    public Category (int id, string name)
    {
      this.Id = id;
      this.Name = name;
    }

    public long Id { get; internal set; }

    public string Name { get; set; }
  }
}
