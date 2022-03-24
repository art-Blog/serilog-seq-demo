namespace seq_demo.Models;

public class JsonResponse<T>
{
    public JsonResponse()
    {
        this.Success = true;
        this.Msg = string.Empty;
    }

    public JsonResponse(T data) : this()
    {
        this.Data = data;
    }

    public JsonResponse(T data, string msg, bool isSuccess)
    {
        this.Success = isSuccess;
        this.Msg = msg;
        this.Data = data;
    }

    /// <summary>
    /// 回傳資料
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// 回傳訊息
    /// </summary>
    public string? Msg { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Success { get; set; }
}