using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Data;
using UniversityApi.Dtos;

namespace Services.Dtos.Mappers
{
    public class GroupMapper
    {
        public static Group MapFromCreateDtoToEntity(GroupGetDto dto)
        {
            return new Group
            {
                Limit = dto.Limit,
                No = dto.No,
            };
        }

        public static GroupGetDto MapFromEntityToGetDto(Group entity)
        {
            return new GroupGetDto
            {
                Id = entity.Id,
                No = entity.No,
                Limit = entity.Limit
            };
        }
    }

}
