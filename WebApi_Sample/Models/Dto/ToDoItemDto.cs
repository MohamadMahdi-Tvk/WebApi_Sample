namespace WebApi_Sample.Models.Dto;

public class ToDoItemDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime InsertTime { get; set; }
    public List<Links> Links { get; set; }
}
