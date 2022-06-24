using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.Data;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace g2hotel_server.Services.Implements
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CustomerRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Customer AddCustomer(Customer customer)
        {
            return _context.Customers.Add(customer).Entity;
        }

        public void Delete(Customer customer)
        {
            _context.Remove(customer);
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.FindAsync(id) ?? throw new Exception("Customer not found");
        }

        public async Task<IEnumerable<Customer>> GetCustomerTasksAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public void Update(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
        }
    }
}