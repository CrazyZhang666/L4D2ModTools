using L4D2ModTools.Data;
using L4D2ModTools.Utils;

namespace L4D2ModTools.Core;

public static class Compile
{
    /// <summary>
    /// 清空求生之路2编译缓存
    /// </summary>
    public static void ClearCache()
    {
        FileUtil.DeleteFileByExtension(Globals.L4D2SurvivorsDir, ".ani");
        FileUtil.DeleteFileByExtension(Globals.L4D2SurvivorsDir, ".vtx");
        FileUtil.DeleteFileByExtension(Globals.L4D2SurvivorsDir, ".mdl");
        FileUtil.DeleteFileByExtension(Globals.L4D2SurvivorsDir, ".vvd");

        FileUtil.DeleteFileByExtension(Globals.L4D2WeaponsDir, ".vtx");
        FileUtil.DeleteFileByExtension(Globals.L4D2WeaponsDir, ".mdl");
        FileUtil.DeleteFileByExtension(Globals.L4D2WeaponsDir, ".vvd");
    }

    /// <summary>
    /// 获取角色模型信息
    /// </summary>
    /// <param name="survivor"></param>
    /// <returns></returns>
    public static ModelInfo GetModelInfo(Survivor survivor)
    {
        var model = new ModelInfo();

        switch (survivor)
        {
            // 一代生还者
            case Survivor.Bill:
                model.Model1 = "survivor_namvet";
                model.Model2 = "v_arms_bill";
                break;

            case Survivor.Francis:
                model.Model1 = "survivor_biker";
                model.Model2 = "v_arms_francis";
                break;

            case Survivor.Louis:
                model.Model1 = "survivor_manager";
                model.Model2 = "v_arms_louis";
                break;

            case Survivor.Zoey:
                model.Model1 = "survivor_teenangst";
                model.Model2 = "v_arms_zoey";
                break;

            // 二代生还者
            case Survivor.Coach:
                model.Model1 = "survivor_coach";
                model.Model2 = "v_arms_coach_new";
                break;

            case Survivor.Ellis:
                model.Model1 = "survivor_mechanic";
                model.Model2 = "v_arms_mechanic_new";
                break;

            case Survivor.Nick:
                model.Model1 = "survivor_gambler";
                model.Model2 = "v_arms_gambler_new";
                break;

            case Survivor.Rochelle:
                model.Model1 = "survivor_producer";
                model.Model2 = "v_arms_producer_new";
                break;
        }

        return model;
    }

    /// <summary>
    /// 判断模型是否存在缩骨文件
    /// </summary>
    /// <returns></returns>
    public static bool IsExistProportions()
    {
        var files = Directory.GetFiles(Globals.AppSurvivorsDir);
        return Array.IndexOf(files, "a_proportions.smd") != -1 ||
            Array.IndexOf(files, "a_proportions_corrective_animation.smd") != -1 ||
            Array.IndexOf(files, "reference.smd") != -1;
    }

    /// <summary>
    /// 运行求生之路2开发者工具，附带运行参数
    /// </summary>
    /// <param name="execPath"></param>
    /// <param name="arguments"></param>
    public static void RunL4D2DevExec(string execPath, string arguments)
    {
        var process = new Process();
        process.StartInfo.FileName = execPath;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        process.Start();
        process.WaitForExit();
    }

    /// <summary>
    /// 运行 studiomdl.exe，附带运行参数
    /// </summary>
    /// <param name="modelAnims"></param>
    /// <param name="survivor"></param>
    public static void StudiomdlExec(Survivor modelAnims, Survivor survivor, string outputDir)
    {
        if (IsExistProportions())
            RunL4D2DevExec(Globals.StudiomdlExec, $"{Globals.L4D2ComplieArgs} {Globals.AppSurvivorsDir}\\{modelAnims}2{survivor}.qc");
        else
            RunL4D2DevExec(Globals.StudiomdlExec, $"{Globals.L4D2ComplieArgs} {Globals.AppSurvivorsDir}\\{modelAnims}2{survivor}_NoProportions.qc");

        var modelInfo = GetModelInfo(survivor);

        // 复制编译完成角色模型
        FileUtil.SafeCopy($"{Globals.L4D2SurvivorsDir}\\{modelInfo.Model1}.dx90.vtx", $"{outputDir}\\models\\survivors\\{modelInfo.Model1}.dx90.vtx");
        FileUtil.SafeCopy($"{Globals.L4D2SurvivorsDir}\\{modelInfo.Model1}.mdl", $"{outputDir}\\models\\survivors\\{modelInfo.Model1}.mdl");
        FileUtil.SafeCopy($"{Globals.L4D2SurvivorsDir}\\{modelInfo.Model1}.vvd", $"{outputDir}\\models\\survivors\\{modelInfo.Model1}.vvd");
    }

    /// <summary>
    /// 运行 studiomdl.exe，附带运行参数
    /// </summary>
    /// <param name="modelAnims"></param>
    /// <param name="survivor"></param>
    public static void StudiomdlExecLight(Survivor modelAnims, Survivor survivor, string outputDir)
    {
        if (IsExistProportions())
            RunL4D2DevExec(Globals.StudiomdlExec, $"{Globals.L4D2ComplieArgs} {Globals.AppSurvivorsDir}\\{modelAnims}2{survivor}_Light.qc");
        else
            RunL4D2DevExec(Globals.StudiomdlExec, $"{Globals.L4D2ComplieArgs} {Globals.AppSurvivorsDir}\\{modelAnims}2{survivor}_Light_NoProportions.qc");

        var modelInfo = GetModelInfo(survivor);

        // 复制编译完成角色Light模型
        FileUtil.SafeCopy($"{Globals.L4D2SurvivorsDir}\\{modelInfo.Model1}_light.ani", $"{outputDir}\\models\\survivors\\{modelInfo.Model1}_light.ani");
        FileUtil.SafeCopy($"{Globals.L4D2SurvivorsDir}\\{modelInfo.Model1}_light.dx90.vtx", $"{outputDir}\\models\\survivors\\{modelInfo.Model1}_light.dx90.vtx");
        FileUtil.SafeCopy($"{Globals.L4D2SurvivorsDir}\\{modelInfo.Model1}_light.mdl", $"{outputDir}\\models\\survivors\\{modelInfo.Model1}_light.mdl");
        FileUtil.SafeCopy($"{Globals.L4D2SurvivorsDir}\\{modelInfo.Model1}_light.vvd", $"{outputDir}\\models\\survivors\\{modelInfo.Model1}_light.vvd");
    }

    /// <summary>
    /// 运行 studiomdl.exe，附带运行参数
    /// </summary>
    /// <param name="survivor"></param>
    public static void StudiomdlExecArms(Survivor survivor, string outputDir)
    {
        RunL4D2DevExec(Globals.StudiomdlExec, $"{Globals.L4D2ComplieArgs} {Globals.AppWeaponsDir}\\{survivor}.qc");

        var modelInfo = GetModelInfo(survivor);

        // 复制编译完成角色手臂
        FileUtil.SafeCopy($"{Globals.L4D2WeaponsDir}\\{modelInfo.Model2}.dx90.vtx", $"{outputDir}\\models\\weapons\\arms\\{modelInfo.Model2}.dx90.vtx");
        FileUtil.SafeCopy($"{Globals.L4D2WeaponsDir}\\{modelInfo.Model2}.mdl", $"{outputDir}\\models\\weapons\\arms\\{modelInfo.Model2}.mdl");
        FileUtil.SafeCopy($"{Globals.L4D2WeaponsDir}\\{modelInfo.Model2}.vvd", $"{outputDir}\\models\\weapons\\arms\\{modelInfo.Model2}.vvd");
    }
}