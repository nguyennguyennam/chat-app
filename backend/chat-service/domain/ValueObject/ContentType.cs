namespace chat_service.domain.ValueObject;

public enum ContentTypeEnum
{
        Text,
        Image,
        File
}
public class ContentType
{

    public ContentTypeEnum Value { get; private set; }
    public ContentType(string contentType)
    {
        if (Enum.TryParse<ContentTypeEnum>(contentType, true, out var result))
        {
            Value = result;
        }
        else
        {
            throw new ArgumentException("Invalid content type");
        }
    }
}