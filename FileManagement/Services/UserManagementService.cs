using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FileManagement.Model;
using FileManagement.Models;
using Newtonsoft.Json;

namespace FileManagement.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly HttpClient client;

        public UserManagementService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<ApplicationUser> GetCurrentUser(string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"/api/user/currentuser");
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ApplicationUser>(responseBody);
        }
        public async Task<UserGroup> AddUserGroup(string token, UserGroupModel userGroupModel)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsJsonAsync($"/api/usergroups",userGroupModel);
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserGroup>(responseBody);
        }
        public async Task<Response> AddToUserGroup(string token, int userGroupId)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsJsonAsync($"/api/usergroupmapping", userGroupId);
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Response>(responseBody);
        }

        public async Task<UserGroupMappingModel> GetUserGroupById(string token, int userGroupId)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"/api/usergroupmapping/{userGroupId}");
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserGroupMappingModel>(responseBody);
        }

        public async Task<IEnumerable<UserGroup>> GetUserGroupByUserId(string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"/api/usergroupmapping/currentUserGroup");
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<UserGroup>>(responseBody);
        }

        public async Task<string> DeleteGroupByUserGroupId(string token, int userGroupId)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync($"/api/usergroups/{userGroupId}");
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        public async Task<string> RestoreGroupByUserGroupId(string token, int userGroupId)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync($"/api/usergroups/restore/{userGroupId}");
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
