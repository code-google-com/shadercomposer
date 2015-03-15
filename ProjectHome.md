# Description #

Shader composer is a universal tool to prototype and develop shader programs.

It is based on a graph-based composition method that allows developers to quickly and interactively prototype shader programs as networks of nodes. This method takes advantage of the graph hierarchical composition, allowing developers to design their shaders at various levels of abstraction, effectively promoting node reusability. Furthermore, consistently building upon the graph's structure and semantics, the method significantly eases shader program debugging and provides very helpful insight into its workings, as demonstrated by the use and results of the open source prototype system implemented.

This method provides an efficient and very intuitive alternative for developing shaders. It is therefore particularly appropriate for novice shader programmers and students.

Furthermore the tool is easily extensible to work with custom renderers or custom nodes.

<a href='http://img408.imageshack.us/img408/2329/sc1t.png'><img src='http://img408.imageshack.us/img408/2329/sc1t.png' width='360px' height='215' /></a>

Note: Currently the DirectX SDK is required for this project to run correctly.

Note: When running the project from source make sure the working directory is set to the top level folder. Libraries and renderers are loaded from the relative paths '../Libraries' and '../Renderers'.