using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using serverProjectFinal.Models;
using serverProjectFinal.BLL;
using AutoMapper;
using serverProjectFinal.DTO;
using Microsoft.AspNetCore.Authorization;
using serverProjectFinal.DAL;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace serverProjectFinal.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donor;
        private readonly IMapper _mapper;

        public DonorController(IDonorService donor, IMapper mapper)
        {
            _donor = donor ?? throw new ArgumentNullException(nameof(donor));
            _mapper = mapper;

        }
       [Authorize(Roles = "manager")] 
        [Route("GetDonor")]
        [HttpGet]
        //הצגת כל התורמים
        public async Task<ActionResult<List<DonorDTO>>> GetDonor()
        {
            IEnumerable<Donor> donors = await _donor.GetDonorBll();
            IEnumerable<DonorDTO> DonorsDTO = _mapper.Map<IEnumerable<Donor>, IEnumerable<DonorDTO>>(donors);
            if (DonorsDTO.Count() == 0)
            {
                return NotFound("User not found");

            }

            return Ok(DonorsDTO);
        }
        [Authorize(Roles = "manager")] 
        [Route("AddDonor")]
        [HttpPost]

        public async Task<ActionResult> AddDonor([FromBody] DonorDTO donor)
        {
            try { 
            Donor newDonor = _mapper.Map<DonorDTO, Donor>(donor);
            DonorDTO newDonorReturn = _mapper.Map<Donor, DonorDTO>(await _donor.AddDonorBll(newDonor));
            if (newDonorReturn == null)
            {
                return NoContent();
            }

            return Ok(newDonorReturn);
            }
            catch (AuthorizationException)
            {
                return Forbid("You don't have the required role to access this resource.");
            }
        }

        [Authorize(Roles = "manager")] 
        [Route("UpdateDonor/{id}")]
        [HttpPut]

        public async Task<string> UpdateDonor(int id, [FromBody] DonorDTO donorDTO)
        {
            Donor donorToUpdate = _mapper.Map<Donor>(donorDTO);
            return await _donor.UpdateDonorBll(id, donorToUpdate);
        }


        [Authorize(Roles = "manager")] 
        [Route("DeleteDonor/{id}")]
        [HttpDelete]
        public async Task<List<Donor>> DeleteDonor(int id)
        {
            return await _donor.DeleteDonorBll(id);
        }

        [Authorize(Roles = "manager")]
        [Route("SelectDonorByNameEmailProduct")]
        [HttpGet]
        public async Task<IEnumerable<Donor>> SelectDonorByNameEmailProduct(string? name, string? email,  string? productName)
        {
            return await _donor.SelectDonorByNameEmailProductBll(name, email, productName);
        }



        [Authorize(Roles = "manager")]
        [Route("GetDonorDetails/{donorId}")]
        [HttpGet]
        public async Task<List<string>> GetDonorDetails(int donorId)
        {
            return await  _donor.GetDonorDetailsBll(donorId);
        }



    }
}
