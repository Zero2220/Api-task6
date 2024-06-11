﻿using Data.Repositories.İnterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Dtos.Mappers;
using Services.Exceptions;
using Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Data;
using UniversityApi.Dtos;
using UniversityApi.Migrations;

namespace Services.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository) 
        {
             _groupRepository = groupRepository;
        }
        public int Create(CreateDto createDto)
        {
            if(createDto == null) throw new ArgumentNullException(nameof(createDto));
            
                if (_groupRepository.Exists(x => x.No == createDto.No))
                    throw new DublicateEntityException();

            Group group = new Group()
            {
                IsDeleted = false,
                Limit = createDto.Limit,
                No = createDto.No,
            };

            _groupRepository.Add(group);
            _groupRepository.Save();

            return group.Id;
        }

        public int Delete(int id)
        {
           if(id < 0) throw new InvalidIdEntityException();

           Group group = _groupRepository.Get(x => x.Id == id && x.IsDeleted !=true);

            group.IsDeleted = true;

            _groupRepository.Save();

            return group.Id;
        }

        public int Edit(int id ,EditDto editDto)
        {

            if (editDto == null) throw new ArgumentNullException();

            if(editDto == null)throw new ArgumentNullException(nameof(editDto));

            var existGroup = _groupRepository.Get(x=>x.Id == id);

            if(editDto.No == existGroup.No && _groupRepository.Exists(x => x.Id == id))
            throw new DublicateEntityException(editDto.No);
            

            existGroup.No = editDto.No;
            existGroup.Limit = editDto.Limit;


            _groupRepository.Save();
            return existGroup.Id;
        }

        public List<GroupGetDto> GetAll(string? search = null)
        {


            return _groupRepository.GetAll(x => x.No.Contains(search)).Select(x => new GroupGetDto
            {
                Id = x.Id,
                No = x.No,
                Limit = x.Limit
            }).ToList();
        }

        public GroupGetDto GetById(int id)
        {
            Group entity = _groupRepository.Get(x => x.Id == id && !x.IsDeleted);

            if (entity == null) throw new RestException(StatusCodes.Status404NotFound, "Group not found");


            //return Mapper<Group, GroupGetDto>.Map(entity);
            return GroupMapper.MapFromEntityToGetDto(entity);
        }
    }
}
