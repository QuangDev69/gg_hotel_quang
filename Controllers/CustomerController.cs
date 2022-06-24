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
    public class CustomerController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public CustomerController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(int id)
        {
            var customerEntity = await _unitOfWork.CustomerRepository.GetCustomerById(id);
            var customer = _mapper.Map<CustomerDTO>(customerEntity);
            return Ok(customer);
        }
        [HttpGet("get-customers")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customersEntity = await _unitOfWork.CustomerRepository.GetCustomerTasksAsync();
            var customers = _mapper.Map<IEnumerable<CustomerDTO>>(customersEntity);
            return Ok(customers);
        }
        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> AddCustomer(CustomerDTO customerDTO)
        {
            var customersEntity = _mapper.Map<Customer>(customerDTO);
            var result = _unitOfWork.CustomerRepository.AddCustomer(customersEntity);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Problem adding customer");   
        }
        [HttpDelete("delete-customer/{id}")]
        public async Task<ActionResult<CustomerDTO>> DeleteCustomer(int id)
        {
            var customersEntity = await _unitOfWork.CustomerRepository.GetCustomerById(id);
            if (customersEntity == null)
            {
                return NotFound();
            }
            _unitOfWork.CustomerRepository.Delete(customersEntity);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Problem deleting customer");
        }
        [HttpPut]
        public async Task<ActionResult<CustomerDTO>> UpdateCustomer(CustomerDTO customerDTO)
        {
            var customer = await _unitOfWork.CustomerRepository.GetCustomerById(customerDTO.Id);
            _mapper.Map(customerDTO, customer);
            _unitOfWork.CustomerRepository.Update(customer);
            if (customer == null)
                return NotFound($"Customer with Id = {customerDTO} not found");
            if (await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to update customer");
        }
    }   

}