using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetbundles
{

    [MenuItem("AssetsBundle/Build AssetBundles")]
    static void BuildAllAssetBundles()//进行打包
    {
        string dir = "Assets/AssetBundles";
        //判断该目录是否存在
        if (Directory.Exists(dir) == false)
        {

            Directory.CreateDirectory(dir);//在工程下创建AssetBundles目录
        }
        //参数一为打包到哪个路径，参数二压缩选项  参数三 平台的目标
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        AssetDatabase.Refresh();
    }
}