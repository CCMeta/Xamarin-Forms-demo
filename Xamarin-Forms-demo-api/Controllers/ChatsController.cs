using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin_Forms_demo_api.Models;
using Xamarin_Forms_demo_api.Services;

namespace Xamarin_Forms_demo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : DefaultController
    {
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly ChatHub _chatHub;
        private readonly ChatsRepository _chatsRepository;

        public ChatsController(IHubContext<ChatHub> chatHubContext, ChatHub chatHub,
            ChatsRepository chatsRepository, IHttpContextAccessor context) : base(context)
        {
            _chatHubContext = chatHubContext;
            _chatHub = chatHub;
            _chatsRepository = chatsRepository;
        }

        // GET: api/<ChatsController> 在线的时候websocketPush 那么没在线的留言怎么办呢 一开始连结WS后主动拉取吗？
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] int partner_id, int max_id)
        {
            var result = await _chatsRepository.GetListByPartner(_uid, partner_id, max_id);
            return Ok(result);
        }

        // GET api/<ChatsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ChatsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Chats chat)
        {
            chat.uid = _uid;
            int lastInsertId = await _chatsRepository.Post(chat);
            if (lastInsertId <= 0)
            {
                return BadRequest(chat);
            }
            var lastInsertItem = await _chatsRepository.Get(_uid, lastInsertId);

            //send to partner if he is online
            //await _chatHubContext.Clients.All.SendAsync("Notify", $"Home page loaded at: {DateTime.Now}");
            _ = Task.Run(async () =>
                  await await _chatHub.OnEventChatSend(_uid.ToString(), lastInsertItem.partner_id.ToString(), MessageType.OnEventChatSend.ToString()));

            return Ok(lastInsertItem);
        }

        // PUT api/<ChatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
