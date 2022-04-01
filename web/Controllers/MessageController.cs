using Microsoft.AspNetCore.Mvc;
using seq_demo.Controllers.Base;
using serilog_seq_demo.Core.Module.Message;

namespace seq_demo.Controllers;

public class MessageController : BaseController
{
    private readonly IMessageModule _messageModule;

    public MessageController(IMessageModule messageModule)
    {
        _messageModule = messageModule;
    }

    public IActionResult Index()
    {
        var message = _messageModule.GetMessage();
        return Content(message);
    }
}