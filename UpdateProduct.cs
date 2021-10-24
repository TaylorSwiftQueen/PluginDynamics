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
            Entity preDeleteImage = Context.PreEntityImages["PreImage"];
            string productName = preDeleteImage["name"].ToString();
            IOrganizationService service = ConnectionFactory.GetCrmService();

            EntityCollection productId = RetrieveProduct(productName);

            foreach (Entity product in productId.Entities)
            {
                Guid ProductId = (Guid)product["productid"];
                Service.Delete("product", ProductId);
            }
        }

        private void UpdateProducts()
        {
            Entity preProductImage = Context.PreEntityImages["PreImage"];
            IOrganizationService service = ConnectionFactory.GetCrmService();

            Entity UpdateProduct = new Entity("product");
            UpdateProduct["name"] = preProductImage["name"];
            UpdateProduct["jrcv_categoriadasvagas"] = (OptionSetValue)preProductImage["jrcv_categoriadasvagas"];
            UpdateProduct["jrcv_numerodecandidatos"] = preProductImage["jrcv_numerodecandidatos"];
            UpdateProduct["jrcv_valordotreinamento"] = (Money)preProductImage["jrcv_valordotreinamento"];

            service.Update(UpdateProduct);
        }

        private void CreateProduct()
        {
            Entity product = (Entity)this.Context.InputParameters["Target"];

            string productName = product.Contains("name") ? product["name"].ToString() : string.Empty;
            OptionSetValue categoriaDasVagas = product.Contains("jrcv_categoriadasvagas") ?
                (OptionSetValue)product["jrcv_categoriadasvagas"] : null;
            int numeroDeCandidatos = product.Contains("jrcv_numerodecandidatos") ? (int)product["jrcv_numerodecandidatos"] : 0;
            Money valorDoTreinamento = product.Contains("jrcv_valordotreinamento") ? (Money)product["jrcv_valordotreinamento"] : null;

            IOrganizationService service = ConnectionFactory.GetCrmService();

            Entity createProduct = new Entity("product");
            createProduct["name"] = productName;
            createProduct["jrcv_categoriadasvagas"] = categoriaDasVagas;
            createProduct["jrcv_numerodecandidatos"] = numeroDeCandidatos;
            createProduct["jrcv_valordotreinamento"] = valorDoTreinamento;

            service.Create(createProduct);
        }

        private EntityCollection RetrieveProduct(string name)
        {
            QueryExpression queryRetrieveProduct = new QueryExpression("product");
            queryRetrieveProduct.ColumnSet.AddColumns("productid");
            queryRetrieveProduct.Criteria.AddCondition("name", ConditionOperator.Equal, name);


            return this.Service.RetrieveMultiple(queryRetrieveProduct);
        }
    }
}
