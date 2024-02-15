using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi1.Data;
using WebApi1.Models;

namespace WebApi1.Services
{
    public class ValueRepository
    {
        private readonly AppDbContext _context;

        public ValueRepository(AppDbContext context)
        {
            _context = context;
        }

        /*public void LazyInitializer()
        {
            _context.Database.EnsureCreated();

            if (!_context.Values.Any())
            {
                var values = new Value[]
                {
                    new Value{Name="Doğa"},
                    new Value{Name="Berin"},
                    new Value{Name="Berkin"},
                    new Value{Name="Furkan"},
                    new Value{Name="Zehra"},
                    new Value{Name="Gökdeniz"},
                    new Value{Name="Berk"},
                };

                foreach (var value in values)
                {
                    _context.Values.Add(value);
                }

                _context.SaveChanges();
            }
        }*/

        public IEnumerable<Value> GetAllValues()
        {
            return _context.Values.ToList();
        }

        public async Task<Value> DeleteValue(int id)
        {
            var result = await _context.Values
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.Values.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }

        public async Task<Value> GetValue(int id)
        {
            return await _context.Values.FirstOrDefaultAsync(e => e.Id == id);
        }

        public void UpdateValue(ValueUpdateModel model)
        {
            var existingValue = _context.Values.Find(model.Id);

            if (existingValue == null)
            {
                throw new InvalidOperationException($"Value with Id = {model.Id} not found");
            }

            existingValue.Name = model.Name; 

            _context.SaveChanges();
        }

        public void CreateValue(Value createValue)
        {
            _context.Database.EnsureCreated();

                    _context.Values.Add(createValue);
                Console.WriteLine("burada");

                _context.SaveChanges();
            
        }
    }
}
