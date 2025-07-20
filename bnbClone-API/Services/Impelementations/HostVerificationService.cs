using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bnbClone_API.Services.Impelementations
{

    public class hostVerificationService : IhostVerificationService
    {
        private readonly IUnitOfWork unitOfWork;

        public hostVerificationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HostVerification>> GetAllHostVerification()
        {
          IEnumerable<HostVerification> hosts= await unitOfWork.hostVerification.GetAllAsync();
           return hosts;   

        }


        public async Task<HostVerification> AddHostVerification([FromForm] HostVerificationDTO hostVerification)
        {
            HostVerification host = new HostVerification();
            host.HostId = hostVerification.HostId;
            host.Type = hostVerification.Type;
            host.Status = hostVerification.Status;
            host.VerifiedAt=hostVerification.VerifiedAt;
            host.SubmittedAt=hostVerification.SubmittedAt;


            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
            var FileName1 = $"{Guid.NewGuid()}{Path.GetExtension(hostVerification.DocumentUrl1.FileName)}";
            var FileName2 = $"{Guid.NewGuid()}{Path.GetExtension(hostVerification.DocumentUrl2.FileName)}";

            var FilePath1 = Path.Combine(FolderPath, FileName1);
            var FilePath2 = Path.Combine(FolderPath, FileName2);

            using var FileStream1 = new FileStream(FilePath1, FileMode.CreateNew);
            using var FileStream2 = new FileStream(FilePath2, FileMode.CreateNew);

            hostVerification.DocumentUrl1.CopyTo(FileStream1);
            hostVerification.DocumentUrl2.CopyTo(FileStream2);

           
            host.DocumentUrl1=FileName1;
            host.DocumentUrl2=FileName2;

            await unitOfWork.hostVerification.AddAsync(host);
            await unitOfWork.SaveAsync();



            return host;

            
        }







        public async Task<HostVerification> EditHostVerification(int id , [FromForm] HostVerificationDTO hostVerification)
        {
            HostVerification host =await unitOfWork.hostVerification.GetByIdAsync(id);
            host.HostId = hostVerification.HostId;
            host.Type = hostVerification.Type;
            host.Status = hostVerification.Status;
            host.VerifiedAt = hostVerification.VerifiedAt;
            host.SubmittedAt = hostVerification.SubmittedAt;


            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
            var FileName1 = $"{Guid.NewGuid()}{Path.GetExtension(hostVerification.DocumentUrl1.FileName)}";
            var FileName2 = $"{Guid.NewGuid()}{Path.GetExtension(hostVerification.DocumentUrl2.FileName)}";

            var FilePath1 = Path.Combine(FolderPath, FileName1);
            var FilePath2 = Path.Combine(FolderPath, FileName2);

            using var FileStream1 = new FileStream(FilePath1, FileMode.CreateNew);
            using var FileStream2 = new FileStream(FilePath2, FileMode.CreateNew);

            hostVerification.DocumentUrl1.CopyTo(FileStream1);
            hostVerification.DocumentUrl2.CopyTo(FileStream2);


            host.DocumentUrl1 = FileName1;
            host.DocumentUrl2 = FileName2;


            await unitOfWork.SaveAsync();

            return host;


        }

        
    }
}
