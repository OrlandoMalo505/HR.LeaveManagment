using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
    {
        private readonly IClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public LeaveAllocationService(IClient httpClient, ILocalStorageService localStorageService) :
            base(localStorageService, httpClient)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }
        public async Task<Response<int>> CreateLeaveAllocations(int leaveTypeId)
        {
            try
            {
                var response = new Response<int>();
                CreateLeaveAllocationDTO createLeaveAllocationDTO = new()
                {
                    LeaveTypeId = leaveTypeId
                };
                AddBearerToken();
                var apiResponse = await _client.LeaveAllocationsPOSTAsync(createLeaveAllocationDTO);
                if (apiResponse.Success)
                {
                    response.Success = true;
                }
                else
                {
                    foreach (var error in apiResponse.Errors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }
                return response;
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }
    }
}
