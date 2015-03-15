# Introduction #

This wikipage provides a short introduction for the Shader Composer tool. It will teach you the basics of composing and understanding the basics of a simple GPU shader program.

(note: screenshots will be added soon)

# Creating a simple shader graph #

After we start up the program we create a new shader program file (.scf). Every shader composer file should contain an output node. This node determines the final result of the shader graph. We create a output node by right clicking on the white design plane and choosing 'Libraries -> Default -> Output'.

For now we will link a simple colour to the output node. Right click the round button to the left of 'Final output' and choose 'Constant -> Color', now pick a random colour.

Before we can see the results of this simple shader we have to choose a renderer. For now we will pick the simple teapot renderer. The result should look something like this.

# Creating a Blinn-Phong shader #

Now we will extend the simple colour shader to a phong lighting shader. To do this we will need to add some new nodes to our graph. The picture below depicts the desired graph. New nodes can be added by right clicking on the design surface and picking the desired node from the corresponding node library. Connections between nodes can be established by dragging a line from one connection point to another.

After you have created your shader choose the build option from the file menu and the resulting outputs should be updated.

# Inspecting intermediate values and shader output #

We can inspect intermediate values in the shader by hovering over a connection. This will popup a small window that shows the value that is transfered through that connection. To pin this popup window simply click once, to unpin click again.

It is also possible to inspect the values of the shader output. Simply click on the pixel in the preview window that you would like to inspect. A cross indicates which pixel is currently selected. The value of the pixel is shown below.