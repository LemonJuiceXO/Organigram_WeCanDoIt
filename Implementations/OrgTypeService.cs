using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.Configuration;
using Org.Apps;
using Org.Domains.NodeTypes;
using Org.Storages;


namespace Org.Impl;

public class OrgTypeService : IOrgTypeService
{
    private readonly OrgTypeStorage orgTypeStorage;
    private readonly IRoleService roleService;

    public OrgTypeService(
        IConfiguration configuration,
        IRoleService roleService
    )
    {
        this.roleService = roleService;
        var connectionString = configuration.GetConnectionString("ORGDB");
        orgTypeStorage = new OrgTypeStorage(connectionString);
    }

    public async ValueTask CreateNodeType(NodeType nodeType)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            await validateNodeForInsertion(nodeType);

            await orgTypeStorage.InsertNodeType(nodeType.Id, nodeType.Code, nodeType.Name);
            
            foreach (NodeRole nodeRole in nodeType.Roles)
            {
                await roleService.CreateRole(nodeRole);
            }
            
            foreach (var role in nodeType.Roles) await orgTypeStorage.AddRoleToNodeType(nodeType.Id, role);

            foreach (var child in nodeType.SubNodes) await orgTypeStorage.AddSubNodeToNodeType(nodeType.Id, child);
            
            scope.Complete();
        }
        finally
        {
            scope.Dispose();
        }
    }

    public async ValueTask<NodeType?> GetNodeTypeById(Guid id)
    {
        return await orgTypeStorage.SelectNodeTypeById(id);
    }

    public async ValueTask<NodeType?> GetNodeTypeByCode(string code)
    {
        return await orgTypeStorage.SelectNodeTypeByCode(code);
    }

    public async ValueTask<List<NodeType>> GetNodeTypes()
    {
        return await orgTypeStorage.SelectNodeTypes();
    }

    private async ValueTask validateNodeForInsertion(NodeType nodeType)
    {
        
        if (await GetNodeTypeById(nodeType.Id) is not null)
            throw new Exception("ID du Noeud existe déja");

        if (await GetNodeTypeByCode(nodeType.Code) is not null)
            throw new Exception("Code du noeud existe déja");
        
    }
}