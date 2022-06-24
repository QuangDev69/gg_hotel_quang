using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.DTOs;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace g2hotel_server.Controllers
{
    public class RoomTypeController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoomTypeController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpGet("get-roomTypes")]
        public async Task<ActionResult<IEnumerable<RoomTypeDTO>>> GetRoomTypes()
        {
            var roomTypesEntity = await _unitOfWork.RoomTypeRepository.GetRoomTypeTasksAsync();
            var roomTypes = _mapper.Map<IEnumerable<RoomTypeDTO>>(roomTypesEntity);
            return Ok(roomTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomTypeDTO>> GetRoomTypeById(int id)
        {
            var roomTypeEntity= await _unitOfWork.RoomTypeRepository.GetRoomTypeById(id);
            var roomType= _mapper.Map<RoomTypeDTO>(roomTypeEntity);
            return Ok(roomType);
        }

        [HttpPost]
        public async Task<ActionResult<RoomTypeDTO>> AddRoomType (RoomTypeDTO roomTypeDTO)
        {
            var roomTypesEntity = _mapper.Map<RoomType>(roomTypeDTO);
            var result= _unitOfWork.RoomTypeRepository.AddRoomType(roomTypesEntity);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Problem adding room type");
        }
        
        [HttpDelete("delete-roomType/{id}")]
        public async Task<ActionResult<RoomTypeDTO>> DeleteRoomType(int id)
        {
            var roomTypesEntity = await _unitOfWork.RoomTypeRepository.GetRoomTypeById(id);
            if (roomTypesEntity == null)
            {
                return NotFound();
            }
            _unitOfWork.RoomTypeRepository.Delete(roomTypesEntity);
            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Problem deleting service");
        }
        [HttpPut]
        public async Task<ActionResult<RoomTypeDTO>> UpdateRoomType(RoomTypeDTO roomTypeDTO, int id)
        {
            var roomType = await _unitOfWork.RoomTypeRepository.GetRoomTypeById(roomTypeDTO.Id);
            _mapper.Map(roomTypeDTO, roomType);
            _unitOfWork.RoomTypeRepository.Update(roomType);
            if (id != roomTypeDTO.Id)
                return BadRequest("Employee ID mismatch");
            if (roomType == null)
                return NotFound($"Employee with Id = {id} not found");
            if (await _unitOfWork.Complete()) return NoContent();
                 return BadRequest("Failed to update Room Type");
        }
    }
}