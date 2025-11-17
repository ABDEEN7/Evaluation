using Microsoft.AspNetCore.Mvc;

namespace Evaluation.API.Controllers;
[Route("api/[controller]/[action]")]
public class PlanMocksController : ControllerBase
{// DTO Classes
    public class CreateEvaluationPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PlanTypeId { get; set; }
        public string PlanTypeName { get; set; }
        public string PlanTypeBackendName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? SemesterId { get; set; }
        public string SemesterName { get; set; }
        public List<PlanSchoolDto> Schools { get; set; }
    }

    public class PlanSchoolDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartEvaluationDate { get; set; }
        public DateTime? EndEvaluationDate { get; set; }
        public Guid? VisitTypeId { get; set; }
        public string VisitTypeName { get; set; }
    }

    public class UpdatePlanRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PlanTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? SemesterId { get; set; }
        public List<UpdateSchoolDto> Schools { get; set; }
    }

    public class UpdateSchoolDto
    {
        public Guid Id { get; set; }
        public DateTime? StartEvaluationDate { get; set; }
        public DateTime? EndEvaluationDate { get; set; }
        public Guid? VisitTypeId { get; set; }
    }

    public class ResponseSchools
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? LastEvaluationDate { get; set; }
        public string Rating { get; set; }
        public int AcademicYear { get; set; }
    }

    // Controller Implementation
    [HttpGet]
    public IActionResult GetPlanById(Guid planId)
    {
        try
        {
            // Mock plan types for reference
            var planTypes = GetMockPlanTypes();
            var semesters = GetMockSemesters();
            var visitTypes = GetMockVisitTypes();
            var allSchools = GetListSchool();

            // Mock plan data based on planId
            var plan = GetMockPlanData(planId, planTypes, semesters, visitTypes, allSchools);

            if (plan == null)
            {
                return NotFound(new { message = "Plan not found" });
            }

            return Ok(new { result = plan });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving plan", error = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult Update([FromBody] UpdatePlanRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { message = "Invalid request data" });
            }

            // Validate required fields
            if (request.Id == Guid.Empty)
            {
                return BadRequest(new { message = "Plan ID is required" });
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest(new { message = "Plan name is required" });
            }

            if (request.PlanTypeId == Guid.Empty)
            {
                return BadRequest(new { message = "Plan type is required" });
            }

            if (request.Schools == null || !request.Schools.Any())
            {
                return BadRequest(new { message = "At least one school must be selected" });
            }

            // Mock: Simulate update operation
            var planTypes = GetMockPlanTypes();
            var planType = planTypes.FirstOrDefault(pt => pt.Id == request.PlanTypeId);
            var semesters = GetMockSemesters();
            var visitTypes = GetMockVisitTypes();
            var allSchools = GetListSchool();

            // Create updated plan DTO
            var updatedPlan = new CreateEvaluationPlanDto
            {
                Id = request.Id,
                Name = request.Name,
                PlanTypeId = request.PlanTypeId,
                PlanTypeName = planType?.Name ?? "Unknown",
                PlanTypeBackendName = planType?.BackendName ?? "Custom",
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                SemesterId = request.SemesterId,
                SemesterName = request.SemesterId.HasValue
                    ? semesters.FirstOrDefault(s => s.Id == request.SemesterId)?.Name
                    : null,
                Schools = request.Schools.Select(s =>
                {
                    var school = allSchools.FirstOrDefault(sch => sch.Id == s.Id);
                    var visitType = visitTypes.FirstOrDefault(vt => vt.Id == s.VisitTypeId);

                    return new PlanSchoolDto
                    {
                        Id = s.Id,
                        Name = school?.Name ?? "Unknown School",
                        StartEvaluationDate = s.StartEvaluationDate,
                        EndEvaluationDate = s.EndEvaluationDate,
                        VisitTypeId = s.VisitTypeId,
                        VisitTypeName = visitType?.Name ?? ""
                    };
                }).ToList()
            };

            return Ok(new
            {
                success = true,
                message = "Plan updated successfully",
                result = updatedPlan
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating plan", error = ex.Message });
        }
    }

    // Helper Methods
    private List<ResponseSchools> GetListSchool()
    {
        return new List<ResponseSchools>
    {
        new ResponseSchools
        {
            Id = new Guid("921d891a-e0cb-4fd4-8e53-fb3443ef0199"),
            Name = "Greenwood High School",
            LastEvaluationDate = new DateTime(2024, 5, 20),
            Rating = "Perfect",
            AcademicYear = 2025
        },
        new ResponseSchools
        {
            Id = new Guid("ecd11007-2ff6-42cb-bca2-b168de94afbc"),
            Name = "Sunrise Elementary",
            LastEvaluationDate = new DateTime(2023, 11, 10),
            Rating = "Week",
            AcademicYear = 2025
        },
        new ResponseSchools
        {
            Id = new Guid("a1b2c3d4-e5f6-4a5b-8c7d-9e0f1a2b3c4d"),
            Name = "Riverside Middle School",
            LastEvaluationDate = new DateTime(2024, 8, 15),
            Rating = "VeryGood",
            AcademicYear = 2025
        },
        new ResponseSchools
        {
            Id = new Guid("f1e2d3c4-b5a6-4978-8c7d-6e5f4a3b2c1d"),
            Name = "Mountainview Academy",
            LastEvaluationDate = new DateTime(2022, 12, 30),
            Rating = "Perfect",
            AcademicYear = 2025
        },
        new ResponseSchools
        {
            Id = new Guid("9a8b7c6d-5e4f-4321-8a7b-6c5d4e3f2a1b"),
            Name = "Lakeside Primary",
            LastEvaluationDate = null,
            Rating = "Aecctable",
            AcademicYear = 2025
        }
    };
    }

    private List<PlanTypeDto> GetMockPlanTypes()
    {
        return new List<PlanTypeDto>
    {
        new PlanTypeDto { Id = new Guid("10000000-0000-0000-0000-000000000001"), Name = "سنوي", BackendName = "Year" },
        new PlanTypeDto { Id = new Guid("20000000-0000-0000-0000-000000000002"), Name = "شهري", BackendName = "Month" },
        new PlanTypeDto { Id = new Guid("30000000-0000-0000-0000-000000000003"), Name = "فصلي", BackendName = "Semester" },
        new PlanTypeDto { Id = new Guid("40000000-0000-0000-0000-000000000004"), Name = "مخصص", BackendName = "Custom" }
    };
    }

    private List<SemesterDto> GetMockSemesters()
    {
        return new List<SemesterDto>
    {
        new SemesterDto
        {
            Id = new Guid("50000000-0000-0000-0000-000000000001"),
            Name = "الفصل الأول",
            StartDate = new DateTime(2025, 9, 1),
            EndDate = new DateTime(2025, 12, 31)
        },
        new SemesterDto
        {
            Id = new Guid("50000000-0000-0000-0000-000000000002"),
            Name = "الفصل الثاني",
            StartDate = new DateTime(2026, 1, 1),
            EndDate = new DateTime(2026, 5, 31)
        }
    };
    }

    private List<VisitTypeDto> GetMockVisitTypes()
    {
        return new List<VisitTypeDto>
    {
        new VisitTypeDto { Id = new Guid("60000000-0000-0000-0000-000000000001"), Name = "دوري" },
        new VisitTypeDto { Id = new Guid("60000000-0000-0000-0000-000000000002"), Name = "استثنائي" },
        new VisitTypeDto { Id = new Guid("60000000-0000-0000-0000-000000000003"), Name = "زيارة" }
    };
    }

    private CreateEvaluationPlanDto GetMockPlanData(Guid planId, List<PlanTypeDto> planTypes,
        List<SemesterDto> semesters, List<VisitTypeDto> visitTypes, List<ResponseSchools> allSchools)
    {
        // Mock different plans based on planId
        var mockPlans = new Dictionary<Guid, CreateEvaluationPlanDto>
    {
        {
            new Guid("11111111-1111-1111-1111-111111111111"),
            new CreateEvaluationPlanDto
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "خطة التقييم السنوية 2025",
                PlanTypeId = planTypes[0].Id, // Year
                PlanTypeName = planTypes[0].Name,
                PlanTypeBackendName = planTypes[0].BackendName,
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 12, 31),
                SemesterId = null,
                Schools = new List<PlanSchoolDto>
                {
                    new PlanSchoolDto
                    {
                        Id = allSchools[0].Id,
                        Name = allSchools[0].Name,
                        StartEvaluationDate = new DateTime(2025, 2, 15),
                        EndEvaluationDate = new DateTime(2025, 2, 20),
                        VisitTypeId = visitTypes[0].Id,
                        VisitTypeName = visitTypes[0].Name
                    },
                    new PlanSchoolDto
                    {
                        Id = allSchools[1].Id,
                        Name = allSchools[1].Name,
                        StartEvaluationDate = new DateTime(2025, 3, 10),
                        EndEvaluationDate = new DateTime(2025, 3, 15),
                        VisitTypeId = visitTypes[1].Id,
                        VisitTypeName = visitTypes[1].Name
                    }
                }
            }
        },
        {
            new Guid("22222222-2222-2222-2222-222222222222"),
            new CreateEvaluationPlanDto
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "خطة الفصل الدراسي الأول",
                PlanTypeId = planTypes[2].Id, // Semester
                PlanTypeName = planTypes[2].Name,
                PlanTypeBackendName = planTypes[2].BackendName,
                StartDate = new DateTime(2025, 9, 1),
                EndDate = new DateTime(2025, 12, 31),
                SemesterId = semesters[0].Id,
                SemesterName = semesters[0].Name,
                Schools = new List<PlanSchoolDto>
                {
                    new PlanSchoolDto
                    {
                        Id = allSchools[2].Id,
                        Name = allSchools[2].Name,
                        StartEvaluationDate = new DateTime(2025, 10, 5),
                        EndEvaluationDate = new DateTime(2025, 10, 10),
                        VisitTypeId = visitTypes[2].Id,
                        VisitTypeName = visitTypes[2].Name
                    },
                    new PlanSchoolDto
                    {
                        Id = allSchools[3].Id,
                        Name = allSchools[3].Name,
                        StartEvaluationDate = new DateTime(2025, 11, 15),
                        EndEvaluationDate = new DateTime(2025, 11, 20),
                        VisitTypeId = visitTypes[0].Id,
                        VisitTypeName = visitTypes[0].Name
                    },
                    new PlanSchoolDto
                    {
                        Id = allSchools[4].Id,
                        Name = allSchools[4].Name,
                        StartEvaluationDate = new DateTime(2025, 12, 1),
                        EndEvaluationDate = new DateTime(2025, 12, 5),
                        VisitTypeId = visitTypes[1].Id,
                        VisitTypeName = visitTypes[1].Name
                    }
                }
            }
        }
    };

        // Return the plan if it exists, otherwise return a default plan
        if (mockPlans.ContainsKey(planId))
        {
            return mockPlans[planId];
        }

        // Default plan for any other ID
        return new CreateEvaluationPlanDto
        {
            Id = planId,
            Name = "خطة مخصصة",
            PlanTypeId = planTypes[3].Id, // Custom
            PlanTypeName = planTypes[3].Name,
            PlanTypeBackendName = planTypes[3].BackendName,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(3),
            Schools = new List<PlanSchoolDto>
        {
            new PlanSchoolDto
            {
                Id = allSchools[0].Id,
                Name = allSchools[0].Name,
                StartEvaluationDate = DateTime.Now.AddDays(7),
                EndEvaluationDate = DateTime.Now.AddDays(10),
                VisitTypeId = visitTypes[0].Id,
                VisitTypeName = visitTypes[0].Name
            }
        }
        };
    }

    // Supporting DTOs
    public class PlanTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BackendName { get; set; }
    }

    public class SemesterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class VisitTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
