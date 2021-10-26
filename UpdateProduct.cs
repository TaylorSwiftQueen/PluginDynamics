using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDynamics1
{

    
    public class UptadeProduct : PluginImplementation
    {
        public string TableName = "product";

        public override void ExecutePlugin(IServiceProvider serviceProvider)
        {
            if (Context.MessageName == "Create")
            {
                CreateProduct();

            }

            else if (Context.MessageName == "Update")
            {
                UpdateProducts();

            }
            else if (Context.MessageName == "Delete")
            {
                DeleteProduct();

            }

        }

        private void DeleteProduct()        
        {
            //FUNÇÃO PARA DELETAR PRODUTOS NO DYNAMICS2
            
            Entity preDeleteImage = Context.PreEntityImages["PreImage"];
            string productName = preDeleteImage["name"].ToString();
            //PEGAR NOME DO PRODUTO PELA PRE-IMAGE

            IOrganizationService service = ConnectionFactory.GetCrmService();

            EntityCollection productId = RetrieveProduct(productName);
            //CHAMAR FUNÇÃO PARA PEGAR O ID DO PRODUTO DO DYNAMICS2 

            foreach (Entity product in productId.Entities)
            {
                Guid ProductId = (Guid)product["productid"];
                service.Delete(TableName, ProductId);
            }
        }

        private void UpdateProducts()
        {
            //FUNÇÃO PARA ATUALIZAR DOS PRODUTOS
            Entity preProductImage = Context.PreEntityImages["PreImage"];
            string productName = preProductImage["name"].ToString();
            
            IOrganizationService service = ConnectionFactory.GetCrmService();

            EntityCollection RetrieveProductId = RetrieveProduct(productName);
            //CHAMAR FUNÇÃO PARA PEGAR O ID DO PRODUTO DO DYNAMICS2 

            foreach (Entity RetrieveProductsId in RetrieveProductId.Entities)
            {
                Guid productId = (Guid)RetrieveProductsId["productid"];               
                Entity UpdateProduct = new Entity(TableName);
                UpdateProduct.Id = productId;
                UpdateProduct["name"] = preProductImage["name"];
                UpdateProduct["jrcv_categoriadasvagas"] = (OptionSetValue)preProductImage["jrcv_categoriadasvagas"];
                UpdateProduct["jrcv_numerodecandidatos"] = preProductImage["jrcv_numerodecandidatos"];
                UpdateProduct["jrcv_valordotreinamento"] = (Money)preProductImage["jrcv_valordotreinamento"];

                service.Update(UpdateProduct);
            }
        }
        private void CreateProduct()
        {
            //FUNÇÃO PARA CRIAR PRODUTOS NO DYNAMICS2
            Entity product = (Entity)this.Context.InputParameters["Target"];

            string productName = product.Contains("name") ? product["name"].ToString() : string.Empty;
            OptionSetValue categoriaDasVagas = product.Contains("jrcv_categoriadasvagas") ?
                (OptionSetValue)product["jrcv_categoriadasvagas"] : null;
            int numeroDeCandidatos = product.Contains("jrcv_numerodecandidatos") ? (int)product["jrcv_numerodecandidatos"] : 0;
            Money valorDoTreinamento = product.Contains("jrcv_valordotreinamento") ? (Money)product["jrcv_valordotreinamento"] : null;

            //RETRIEVE DOS ITENS VIA PLUGIN (TARGET)
            
            IOrganizationService service = ConnectionFactory.GetCrmService();

            Entity createProduct = new Entity(TableName);
            createProduct["name"] = productName;
            createProduct["jrcv_categoriadasvagas"] = categoriaDasVagas;
            createProduct["jrcv_numerodecandidatos"] = numeroDeCandidatos;
            createProduct["jrcv_valordotreinamento"] = valorDoTreinamento;

            service.Create(createProduct);
        }

        private EntityCollection RetrieveProduct(string name)
        {
            IOrganizationService service = ConnectionFactory.GetCrmService();
            //FUNÇÃO PARA FAZER RETRIEVE DO ID DO PRODUTO NO DYNAMICS2 POR MEIO DO NOME DO PRODUTO
            QueryExpression queryRetrieveProduct = new QueryExpression(TableName);
            queryRetrieveProduct.ColumnSet.AddColumns("productid");
            queryRetrieveProduct.Criteria.AddCondition("name", ConditionOperator.Equal, name);


            return service.RetrieveMultiple(queryRetrieveProduct);
        }
    }
}
