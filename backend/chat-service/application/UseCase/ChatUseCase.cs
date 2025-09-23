using chat_service.application.DTOs;
using chat_service.application.Interface;
using chat_service.application.Mapper;
using chat_service.domain.Repository;

namespace chat_service.application.UseCase
{
    public class ChatUseCase : IChatUseCase
    {
        private readonly IChatRepository _repository;

        public ChatUseCase(IChatRepository repository)
        {
            _repository = repository;
        }

        /** Send a new message */
        public async Task<MessageDto> SendMessage(SendMessageRequestDto dto)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            // Map DTO -> Domain
            var domainMessage = MessageMapper.FromRequest(dto);

            // Save message
            var saved = await _repository.SendMessage(domainMessage);

            // Map Domain -> DTO
            return MessageMapper.ToDto(saved);
        }

        /** Edit an existing message */
        public async Task<MessageDto> EditMessage(Guid groupId, Guid messageId, string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("newContent must not be empty", nameof(newContent));

            var updated = await _repository.EditMessage(groupId, messageId, newContent);
            return MessageMapper.ToDto(updated);
        }

        /** Delete a message */
        public async Task DeleteMessage(Guid groupId, Guid messageId)
        {
            await _repository.DeleteMessage(groupId, messageId);
        }

        /** Get messages with pagination (scroll style) */
        public async Task<List<MessageDto>> GetMessagesByGroup(Guid groupId, DateTime? before, int pageSize = 15)
        {
            var messages = await _repository.GetMessagesByGroup(groupId, before, pageSize);
            return messages.Select(MessageMapper.ToDto).ToList();
        }

        /** Search messages in a group */
        public async Task<List<MessageDto>> SearchMessage(Guid groupId, string keyword)
        {
            var results = await _repository.SearchMessages(groupId, keyword);
            return results.Select(MessageMapper.ToDto).ToList();
        }
    }
}
