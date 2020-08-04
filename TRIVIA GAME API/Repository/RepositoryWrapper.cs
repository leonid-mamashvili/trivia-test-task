using System;
using System.Collections.Generic;
using System.Text;
using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private TriviaContext _triviaContext;
        private IAnswerRepository _answer;
        private ICategoryRepository _category;
        private IGameplayRoomRepository _gameplayRoom;
        private IPlayerRepository _player;
        private IQuestionRepository _question;

        public IAnswerRepository Answer
        {
            get
            {
                if (_answer == null)
                {
                    _answer = new AnswerRepository(_triviaContext);
                }

                return _answer;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepository(_triviaContext);
                }

                return _category;
            }
        }

        public IGameplayRoomRepository GameplayRoom
        {
            get
            {
                if (_gameplayRoom == null)
                {
                    _gameplayRoom = new GameplayRoomRepository(_triviaContext);
                }

                return _gameplayRoom;
            }
        }

        public IPlayerRepository Player
        {
            get
            {
                if (_player == null)
                {
                    _player = new PlayerRepository(_triviaContext);
                }

                return _player;
            }
        }

        public IQuestionRepository Question
        {
            get
            {
                if (_question == null)
                {
                    _question = new QuestionRepository(_triviaContext);
                }

                return _question;
            }
        }

        public RepositoryWrapper(TriviaContext repositoryContext)
        {
            _triviaContext = repositoryContext;
        }

        public void Save()
        {
            _triviaContext.SaveChanges();
        }
    }
}

