using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tecnicos.Adbtractions;
using Tecnicos.Data.Context;
using Tecnicos.Data.Models;
using Tecnicos.Domain.DTO;

namespace Tecnicos.Services
{
    public class ClientesService(IDbContextFactory<TecnicosContext> DbFactory) : IClientesService
    {
        public async Task<bool> Guardar(ClientesDto cliente)
        {
            if (!await Existe(cliente.ClienteId))
            {
                return await Insertar(cliente);
            }
            else
            {
                return await Modificar(cliente);
            }
        }
        private async Task<bool> Existe(int clienteId)
        {
            await using var _contexto = await DbFactory.CreateDbContextAsync();

            return await _contexto.Clientes
                .AnyAsync(c => c.ClienteId == clienteId);
        }
        private async Task<bool> Insertar(ClientesDto clienteDto)
        {
            await using var _contexto = await DbFactory.CreateDbContextAsync();
            var cliente = new Clientes()
            {
                Nombres = clienteDto.Nombres,
                WhatsApp = clienteDto.WhatsApp
            };
            _contexto.Clientes.Add(cliente);
            var guardar = await _contexto.SaveChangesAsync() > 0;
            clienteDto.ClienteId = cliente.ClienteId;
            return guardar;
        }

        private async Task<bool> Modificar(ClientesDto clienteDto)
        {
            await using var _contexto = await DbFactory.CreateDbContextAsync();
            var cliente = new Clientes()
            {
                ClienteId = clienteDto.ClienteId,
                Nombres = clienteDto.Nombres,
                WhatsApp = clienteDto.WhatsApp
            };
            _contexto.Update(cliente);
            var modificar = await _contexto.SaveChangesAsync() > 0;
            clienteDto.ClienteId = cliente.ClienteId;
            return modificar;

        }

        public async Task<bool> Eliminar(int clienteId)
        {
            await using var _contexto = await DbFactory.CreateDbContextAsync();

            return await _contexto.Clientes
                .Where(c => c.ClienteId == clienteId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<ClientesDto> Buscar(int clienteId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            var cliente = await contexto.Clientes
                .Where(c => c.ClienteId == clienteId)
                .Select(c => new ClientesDto()
                {
                    ClienteId = c.ClienteId,
                    Nombres = c.Nombres,
                    WhatsApp = c.WhatsApp
                }).FirstOrDefaultAsync();

            return cliente ?? new ClientesDto();
        }

        public async Task<bool> ExisteCliente(int clienteId, string nombre, string whatsApp)
        {
            await using var _contexto = await DbFactory.CreateDbContextAsync();

            return await _contexto.Clientes
                .AnyAsync(c => c.ClienteId != clienteId
                && (c.Nombres.ToLower().Equals(nombre.ToLower())
                || c.WhatsApp.ToLower().Equals(whatsApp.ToLower())));
        }



        public async Task<List<ClientesDto>> Listar(Expression<Func<ClientesDto, bool>> criterio)
        {
            await using var _contexto = await DbFactory.CreateDbContextAsync();

            return await _contexto.Clientes
                .Select(c => new ClientesDto()
                {
                    ClienteId = c.ClienteId,
                    Nombres = c.Nombres,
                    WhatsApp = c.WhatsApp
                })
                .Where(criterio)
                .ToListAsync();
        }
    }
}
