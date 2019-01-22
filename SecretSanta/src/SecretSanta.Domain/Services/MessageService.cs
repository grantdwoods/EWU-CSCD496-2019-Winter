using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretSanta.Domain.Services
{
    public class MessageService
    {
        private ApplicationDbContext Context { get; }
        public MessageService(ApplicationDbContext context)
        {
            this.Context = context;
        }

        public Message AddMessage(Message message)
        {
            Context.Messages.Add(message);
            Context.SaveChanges();

            return message;
        }

        public Message Find(int id)
        {
            return Context.Messages
                .Include(message => message.ToUser)
                .Include(message => message.FromUser)
                .SingleOrDefault(message => message.Id == id);
        }
    }
}
