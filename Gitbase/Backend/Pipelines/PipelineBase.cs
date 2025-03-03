using System.Diagnostics;
using Backend.Models;
using Backend.Constants;
using Backend.Pipelines.PLHandlers;

namespace Backend.Pipelines {
    public class PipelineBase {
        protected IPipelineHandler handler;
        protected string systemFamily;
        protected PipelineBase(IConfiguration configuration) {
            systemFamily = configuration[Configuration.SYSTEM_FAMILY_KEY] ?? "NONVALID";
            switch (systemFamily) {
                case SystemTypes.LINUX_TYPE:
                    handler = new LinuxPLHandler(configuration);
                    break;
                case SystemTypes.WIN_DBG_TYPE:
                    handler = new WindowsDbgPLHandler();
                    break;
                default:
                    throw new Exception(ErrMsgs.SYSTEM_FAMILY_CONFIG_IS_NOT_VALID);
            }
        }
        
    }
}
