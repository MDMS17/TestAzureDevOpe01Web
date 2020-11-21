using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TestAzureDevOps01Web.Controllers;

namespace TestAzureDevOps01Tst
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async System.Threading.Tasks.Task TestGetVoteAsync()
        {
            List<KeyValuePair<string, int>> result;
            var controller = new HomeController();
            result = await controller.GetVotes();
            Assert.IsTrue(result.Count > 0);

        }
        [TestMethod]
        public async System.Threading.Tasks.Task TestAddVote()
        {
            List<KeyValuePair<string, int>> result;
            string itemName = "NewItem";
            var controller = new HomeController();
            await controller.UpdateOrAddVote(itemName);
            result = await controller.GetVotes();
            Assert.IsTrue(result.Find(x => x.Key == "NewItem").Value == 1);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task TestUpdateVote()
        {
            List<KeyValuePair<string, int>> result;
            string itemName = "NewItem";
            var controller = new HomeController();
            await controller.UpdateOrAddVote(itemName);
            result = await controller.GetVotes();
            Assert.IsTrue(result.Find(x => x.Key == "NewItem").Value == 2);
        }
        [TestMethod]
        public async System.Threading.Tasks.Task TestDeleteVote()
        {
            List<KeyValuePair<string, int>> result;
            string itemName = "NewItem";
            var controller = new HomeController();
            await controller.DeleteVote(itemName);
            result = await controller.GetVotes();
            Assert.IsFalse(result.Exists(x => x.Key == "NewItem"));
        }
    }
}

