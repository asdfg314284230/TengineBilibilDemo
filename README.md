# TEngine
<p align="center">
    <img src="http://1.12.241.46:8081/temp/TEngine512.png" alt="logo" width="384" height="384">
</p>

<h3 align="center"><strong>此项目是基于Tengine实现的Bilibili直播间访问的Demo<strong></h3>

<p align="center">
  <strong>Unity框架解决方案<strong>
    <br>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/badge/Unity%20Ver-2019.4.12++-blue.svg?style=flat-square" alt="status" />
  </a>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/github/license/ALEXTANGXIAO/TEngine" alt="license" />
  </a>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/github/last-commit/ALEXTANGXIAO/TEngine" alt="last" />
  </a>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/github/issues/ALEXTANGXIAO/TEngine" alt="issue" />
  </a>
  <a style="text-decoration:none">
    <img src="https://img.shields.io/github/languages/top/ALEXTANGXIAO/TEngine" alt="topLanguage" />
  </a>
  <a style="text-decoration:none">
    <img src="https://app.fossa.com/api/projects/git%2Bgithub.com%2FJasonXuDeveloper%2FJEngine.svg?type=shield" alt="status" />
  </a>
  <br>
  
  <br>
</p>


```
//项目结构
Assets
├── link.xml            // IL2CPP的防裁剪
├── TEngine             // 框架目录
├── TResources          // 资源文件目录(可以自己修改AssetConfig进行自定义)
└── HotUpdateScripts    // 热更脚本资源(可以把TEngine下的Runtime脚本放入此处，让TEngine也处于热更域)

TEngine
├── Config~             // 配置表和转表工具(一键转表生成C#结构体和Json配置)
├── FileServer~         // Node编写的资源文件系统，可以部署测试AB下载，生产环境最好还是用OSS
├── UIFrameWork~        // UI系统的Package包
├── Editor              // TEngine编辑器核心代码
└── Runtime             // TEngine核心代码
    ├── PlayerPrefsDataMgr// 本地可持久化(非必要)       
    ├── Audio           // 音频模块(非必要)
    ├── Config          // 配置表加载器(非必要)
    ├── Mono            // Mono管理器
    ├── Unitity         // 工具类
    ├── Res             // 资源加载管理器
    ├── HotUpdate       // 热更新模块(非必要)
    ├── UI              // UI系统模块(非必要)
    ├── Net             // 网络模块(非必要)
    ├── ECS             // ECS模块(非必要)
    ├── Event           // Event事件模块
    └── Core            // 核心模块
```
---


## <strong>技术支持
 QQ群：967860570   
 欢迎大家提供意见和改进意见，不喜请友善提意见哈 谢谢~   
 如果您觉得感兴趣想期待关注一下或者有眼前一亮的模块，不妨给个Star~

 作者:无名
---

## <strong>优质开源项目推荐
#### <a href="https://github.com/ALEXTANGXIAO/TEngine"><strong>JEngine</strong></a> - 使Unity开发的游戏支持热更新的解决方案。

#### <a href="https://github.com/focus-creative-games/hybridclr"><strong>HybridCLR</strong></a> - 特性完整、零成本、高性能、低内存的近乎完美的Unity全平台原生c#热更方案
