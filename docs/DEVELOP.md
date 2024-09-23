# develop

## 先说结论

- 使用 `Godot 4.x mono` 最新正式版本，若有严重 bug fix 可以临时使用 dev 版本
- 使用 `.NET/C#` 编写代码，尽量不使用 C++ 和 GDScript
- 注意 .NET 8 上游微软问题，不支持 Web 导出，需要等待后续更新

## R&D

### 跨平台 > 开发效率(调试) > 体验(性能)

1. Mobile (Android, iOS, iPad) 🌟🌟🌟🌟🌟
2. PC (Windows, macOS, Linux) 🌟🌟🌟
3. XR (Meta Quest, PICO, Apple Vision Pro) 🌟

### 可选 GUI 方案

- Qt (C++) 商业收费
- [Electron (JS)](https://github.com/electron/electron.git)
- [.NET MAUI (C#)](https://github.com/dotnet/maui.git)
- [Taro (JS) 小程序](https://github.com/NervJS/taro.git)
- [React Native (JS)](https://github.com/facebook/react-native.git), [Expo](https://github.com/expo/expo.git)
- [Flutter (Dart)](https://github.com/flutter/flutter.git)

### 可选 3D 方案

- [Godot (GDScript)](https://github.com/godotengine/godot.git), [GDExtension (C++, WASM)](https://github.com/godotengine/godot-cpp.git)
- Unity (C#, WASM) 商业收费
- Unreal Engine (Blueprint/C++) 商业收费
- [three.js](https://github.com/mrdoob/three.js.git), [react-three-fiber](https://github.com/pmndrs/react-three-fiber.git)
- [Babylon.js](https://github.com/BabylonJS/Babylon.js.git)
- [Filament (C++, WASM)](https://github.com/google/filament), [react-native-filament (JS)](https://github.com/margelo/react-native-filament.git)
- Microsoft Havok (C++), [Havok Physics for Web (WASM)](https://github.com/BabylonJS/havok.git)
- [NVIDIA PhysX (C++, WASM)](https://github.com/NVIDIA-Omniverse/PhysX.git)
- [Bullet (C++)](https://github.com/bulletphysics/bullet3.git), [ammo.js (WASM)](https://github.com/kripken/ammo.js.git)
- [Jolt (C++)](https://github.com/jrouwe/JoltPhysics.git), [JoltPhysics.js (WASM)](https://github.com/jrouwe/JoltPhysics.js.git)
- [cannon.js](https://github.com/pmndrs/cannon-es.git)

## 资料

- [Godot - Setup C# debugging](https://cococode.net/courses/how-to-use-cs-in-godot)
- [Godot - as library in Milestone 4.4 (Android)](https://github.com/godotengine/godot/pull/90510)
- [Godot - Web Export in 4.3](https://godotengine.org/article/progress-report-web-export-in-4-3/)
- [Godot - C# platform support in 4.2](https://godotengine.org/article/platform-state-in-csharp-for-godot-4-2/)
- [Godot - GDExtension in 4.0](https://godotengine.org/article/introducing-gd-extensions/)
- [Unity - as library in 2019.3 (Android, iOS, Windows)](https://docs.unity3d.com/Manual/UnityasaLibrary.html)
- [UE - as library in 4.27 (Windows)](https://forums.unrealengine.com/t/ue-4-27-preview-ue-as-a-lib/484701)
- [Babylon - Using Havok and the Havok Plugin](https://doc.babylonjs.com/features/featuresDeepDive/physics/havokPlugin)
- [RN - New Architecture in Beta 0.74](https://reactnative.dev/docs/next/the-new-architecture/landing-page)
- [Flutter - Roadmap](https://github.com/flutter/flutter/blob/master/docs/roadmap/Roadmap.md)
