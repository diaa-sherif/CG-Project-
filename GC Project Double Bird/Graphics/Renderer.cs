using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;


using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint triangleBufferID;
        uint bird;
        uint xyzAxesBufferID;

        //3D Drawing
        mat4 ModelMatrix;
        mat4 ModelMatrix1;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX=0, 
                     translationY=0, 
                     translationZ=0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 triangleCenter;
        vec3 triangleCenter1;

        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            const float x = 10;
            Gl.glClearColor(0.44f, 0.44f, 0.44f, 1);
            
            float[] triangleVertices = { 
                 //0 mouth
		        x*0.95f, x*0.396f, 0.0f,  // 1
                1,1,0,
                x*0.378f, x*0.372f, 0.0f,  // 4
                1,1,0,
                x*0.339f, x*0.449f, 0.0f,  // 5
                1,1,0,
                x*0.339f, x*0.353f, 0.0f,  // 20
                1,1,0,
                x*0.208f, x*0.446f, 0.0f,  // 8
                1,1,0,
                //5 eye
                x*0.339f, x*0.449f, 0.0f,  // 5
                0,0,0,
                x*0.24f, x*0.486f, 0.0f,  // 7
                0,0,0,
                x*0.208f, x*0.446f, 0.0f,  // 8
                0,0,0,
                //8 top head
                x*0.24f, x*0.486f, 0.0f,  // 7
                1,0,0,
                x*0.339f, x*0.449f, 0.0f,  // 5
                1,0,0,
                x*0.24f, x*0.52f, 0.0f,  // 6
                1,0,0,
                x*0.172f, x*0.536f, 0.0f,  // 11
                1,0,0,
                x*0.087f, x*0.539f, 0.0f,  // 12
                1,0,0,
                x*0.02f, x*0.529f, 0.0f,  // 13
                1,0,0,
                x*-0.112f, x*0.452f, 0.0f,  // 14
                1,0,0,
                x*0.208f, x*0.446f, 0.0f,  // 8
                1,0,0,
                //16 neck
                x*0.208f, x*0.446f, 0.0f,  // 8
                0,0,0.7f,
                x*-0.112f, x*0.452f, 0.0f,  // 14
                0,0,0.7f,
                x*-0.119f, x*0.344f, 0.0f,  // 15
                0,0,0.7f,
                x*-0.222f, x*0.263f, 0.0f,  // 28
                0,0,0.7f,
                x*-0.098f, x*0.167f, 0.0f,  // 25
                0,0,0.7f,
                x*0.03f, x*0.186f, 0.0f,  // 23
                0,0,0.7f,
                x*0.133f, x*0.204f, 0.0f,  // 22
                0,0,0.7f,
                x*0.247f, x*0.263f, 0.0f,  // 18
                0,0,0.7f,
                x*0.272f, x*0.297f, 0.0f,  // 19
                0,0,0.7f,
                x*0.339f, x*0.353f, 0.0f,  // 20
                0,0,0.7f,
                //26 stomach
                x*0.155f, x*0.028f, 0.0f,  // 27
                0.8f,1,1,
                x*0.247f, x*0.263f, 0.0f,  // 18
                0.8f,1,1,
                x*0.133f, x*0.204f, 0.0f,  // 22
                0.8f,1,1,
                x*0.03f, x*0.186f, 0.0f,  // 23
                0.8f,1,1,
                x*-0.098f, x*0.167f, 0.0f,  // 25
                0.8f,1,1,
                x*-0.265f, x*0.015f, 0.0f,  // 33
                0.8f,1,1,
                x*-0.425f, x*-0.059f, 0.0f,  // 38
                0.8f,1,1,
                x*-0.336f, x*-0.384f, 0.0f,  // 44
                0.8f,1,1,
                x*-0.14f, x*-0.291f, 0.0f,  // 42
                0.8f,1,1,
                x*0.005f, x*-0.211f, 0.0f,  // 41
                0.8f,1,1,
                x*0.034f, x*-0.183f, 0.0f,  // 30
                0.8f,1,1,
                x*-0.119f, x*-0.043f, 0.0f,  // 29
                0.8f,1,1,
                //38 tail
                x*-0.478f, x*-0.437f, 0.0f,  // 53
                0,0,0.8f,
                x*-0.538f, x*-0.48f, 0.0f,  // 52
                0,0,0.8f,
                x*-0.634f, x*-0.613f, 0.0f,  // 51
                0,0,0.8f,
                x*-0.542f, x*-0.901f, 0.0f,  // 61
                0,0,0.8f,
                x*-0.513f, x*-0.96f, 0.0f,  // 60
                0,0,0.8f,
                x*-0.435f, x*-0.87f, 0.0f,  // 59
                0,0,0.8f,
                x*-0.393f, x*-0.706f, 0.0f,  // 57
                0,0,0.8f,
                x*-0.329f, x*-0.647f, 0.0f,  // 56
                0,0,0.8f,
                x*-0.336f, x*-0.384f, 0.0f,  // 44
                0,0,0.8f,
                //47 back1
                x*-0.478f, x*-0.437f, 0.0f,  // 53
                0,0,0.7f,
                x*-0.336f, x*-0.384f, 0.0f,  // 44
                0,0,0.7f,
                x*-0.425f, x*-0.059f, 0.0f,  // 38
                0,0,0.7f,
                x*-0.599f, x*-0.062f, 0.0f,  // 37
                0,0,0.7f,
                x*-0.648f, x*-0.223f, 0.0f,  // 47
                0,0,0.7f,
                x*-0.655f, x*-0.533f, 0.0f,  // 50
                0,0,0.7f,
                x*-0.538f, x*-0.48f, 0.0f,  // 52
                0,0,0.7f,
                //54 back2
                x*-0.655f, x*-0.533f, 0.0f,  // 50
                0,0,0.7f,
                x*-0.634f, x*-0.613f, 0.0f,  // 51
                0,0,0.7f,
                x*-0.538f, x*-0.48f, 0.0f,  // 52
                0,0,0.7f,
                //57 back3
                x*-0.098f, x*0.167f, 0.0f,  // 25
                0,0,0.75f,
                x*-0.222f, x*0.263f, 0.0f,  // 28
                0,0,0.75f,
                x*-0.247f, x*0.263f, 0.0f,  // 34
                0,0,0.75f,
                x*-0.595f, x*-0.043f, 0.0f,  // 36
                0,0,0.75f,
                x*-0.599f, x*-0.062f, 0.0f,  // 37
                0,0,0.75f,
                x*-0.425f, x*-0.059f, 0.0f,  // 38
                0,0,0.75f,
                x*-0.265f, x*0.015f, 0.0f,  // 33
                0,0,0.75f,
                //64 wings
                x*-0.247f, x*0.263f, 0.0f,  // 34
                0,1,0.9f,
                x*-0.826f, x*0.981f, 0.0f,  //88
                0,0,0.9f,
                x*-0.89f, x*0.928f, 0.0f,  //87
                0,0,0.9f,
                x*-0.922f, x*0.867f, 0.0f,  //84
                0,0,0.9f,
                x*-0.933f, x*0.721f, 0.0f,  //83
                0,0,0.9f,
                x*-0.922f, x*0.638f, 0.0f,  //80
                0,0,0.9f,
                x*-0.901f, x*0.551f, 0.0f,  //79
                0,0,0.9f,
                x*-0.854f, x*0.449f, 0.0f,  //78
                0,0,0.9f,
                x*-0.829f, x*0.378f, 0.0f,  //75
                0,0,0.9f,
                x*-0.794f, x*0.303f, 0.0f,  //74
                0,0,0.9f,
                x*-0.755f, x*0.238f, 0.0f,  //71
                0,0,0.9f,
                x*-0.687f, x*0.099f, 0.0f,  //69
                0,0,0.9f,
                x*-0.595f, x*-0.043f, 0.0f,  // 36
                0,0,0.9f,


            };
            triangleCenter = new vec3(0,0 ,0);
            float[] triangleVertices1 = { 
                 //0 mouth
		        x*0.95f, x*0.396f, 0.0f,  // 1
                0,0,0,
                x*0.378f, x*0.372f, 0.0f,  // 4
                0,0,0,
                x*0.339f, x*0.449f, 0.0f,  // 5
                0,0,0,
                x*0.339f, x*0.353f, 0.0f,  // 20
                0,0,0,
                x*0.208f, x*0.446f, 0.0f,  // 8
                0,0,0,
                //5 eye
                x*0.339f, x*0.449f, 0.0f,  // 5
                0,0,0,
                x*0.24f, x*0.486f, 0.0f,  // 7
                0,0,0,
                x*0.208f, x*0.446f, 0.0f,  // 8
                0,0,0,
                //8 top head
                x*0.24f, x*0.486f, 0.0f,  // 7
                0,0,0,
                x*0.339f, x*0.449f, 0.0f,  // 5
                0,0,0,
                x*0.24f, x*0.52f, 0.0f,  // 6
                0,0,0,
                x*0.172f, x*0.536f, 0.0f,  // 11
                0,0,0,
                x*0.087f, x*0.539f, 0.0f,  // 12
                0,0,0,
                x*0.02f, x*0.529f, 0.0f,  // 13
                0,0,0,
                x*-0.112f, x*0.452f, 0.0f,  // 14
                0,0,0,
                x*0.208f, x*0.446f, 0.0f,  // 8
                0,0,0,
                //16 neck
                x*0.208f, x*0.446f, 0.0f,  // 8
                0,0,0,
                x*-0.112f, x*0.452f, 0.0f,  // 14
                0,0,0,
                x*-0.119f, x*0.344f, 0.0f,  // 15
                0,0,0,
                x*-0.222f, x*0.263f, 0.0f,  // 28
                0,0,0,
                x*-0.098f, x*0.167f, 0.0f,  // 25
                0,0,0,
                x*0.03f, x*0.186f, 0.0f,  // 23
                0,0,0,
                x*0.133f, x*0.204f, 0.0f,  // 22
                0,0,0,
                x*0.247f, x*0.263f, 0.0f,  // 18
                0,0,0,
                x*0.272f, x*0.297f, 0.0f,  // 19
                0,0,0,
                x*0.339f, x*0.353f, 0.0f,  // 20
                0,0,0,
                //26 stomach
                x*0.155f, x*0.028f, 0.0f,  // 27
                1,1,1,
                x*0.247f, x*0.263f, 0.0f,  // 18
                1,1,1,
                x*0.133f, x*0.204f, 0.0f,  // 22
                1,1,1,
                x*0.03f, x*0.186f, 0.0f,  // 23
                1,1,1,
                x*-0.098f, x*0.167f, 0.0f,  // 25
                1,1,1,
                x*-0.265f, x*0.015f, 0.0f,  // 33
                1,1,1,
                x*-0.425f, x*-0.059f, 0.0f,  // 38
                1,1,1,
                x*-0.336f, x*-0.384f, 0.0f,  // 44
                1,1,1,
                x*-0.14f, x*-0.291f, 0.0f,  // 42
                1,1,1,
                x*0.005f, x*-0.211f, 0.0f,  // 41
                1,1,1,
                x*0.034f, x*-0.183f, 0.0f,  // 30
                1,1,1,
                x*-0.119f, x*-0.043f, 0.0f,  // 29
                1,1,1,
                //38 tail
                x*-0.478f, x*-0.437f, 0.0f,  // 53
                0,1,0,
                x*-0.538f, x*-0.48f, 0.0f,  // 52
                0,1,0,
                x*-0.634f, x*-0.613f, 0.0f,  // 51
                0,1,0,
                x*-0.542f, x*-0.901f, 0.0f,  // 61
                0,1,0,
                x*-0.513f, x*-0.96f, 0.0f,  // 60
                0,1,0,
                x*-0.435f, x*-0.87f, 0.0f,  // 59
                0,1,0,
                x*-0.393f, x*-0.706f, 0.0f,  // 57
                0,1,0,
                x*-0.329f, x*-0.647f, 0.0f,  // 56
                0,1,0,
                x*-0.336f, x*-0.384f, 0.0f,  // 44
                0,1,0,
                //47 back1
                x*-0.478f, x*-0.437f, 0.0f,  // 53
                0,1,0,
                x*-0.336f, x*-0.384f, 0.0f,  // 44
                0,1,0,
                x*-0.425f, x*-0.059f, 0.0f,  // 38
                0,1,0,
                x*-0.599f, x*-0.062f, 0.0f,  // 37
                0,1,0,
                x*-0.648f, x*-0.223f, 0.0f,  // 47
                0,1,0,
                x*-0.655f, x*-0.533f, 0.0f,  // 50
                0,1,0,
                x*-0.538f, x*-0.48f, 0.0f,  // 52
                0,1,0,
                //53 back2
                x*-0.655f, x*-0.533f, 0.0f,  // 50
                0,1,0,
                x*-0.634f, x*-0.613f, 0.0f,  // 51
                0,1,0,
                x*-0.538f, x*-0.48f, 0.0f,  // 52
                0,1,0,
                //56 back3
                x*-0.098f, x*0.167f, 0.0f,  // 25
                1,0,0,
                x*-0.222f, x*0.263f, 0.0f,  // 28
                1,0,0,
                x*-0.247f, x*0.263f, 0.0f,  // 34
                1,0,0,
                x*-0.595f, x*-0.043f, 0.0f,  // 36
                1,0,0,
                x*-0.599f, x*-0.062f, 0.0f,  // 37
                1,0,0,
                x*-0.425f, x*-0.059f, 0.0f,  // 38
                1,0,0,
                x*-0.265f, x*0.015f, 0.0f,  // 33
                1,0,0,
                //63 wings
                x*-0.247f, x*0.263f, 0.0f,  // 34
                1,0,0,
                x*-0.826f, x*0.981f, 0.0f,  //88
                1,0,0,
                x*-0.89f, x*0.928f, 0.0f,  //87
                1,0,0,
                x*-0.922f, x*0.867f, 0.0f,  //84
                1,0,0,
                x*-0.933f, x*0.721f, 0.0f,  //83
                1,0,0,
                x*-0.922f, x*0.638f, 0.0f,  //80
                1,0,0,
                x*-0.901f, x*0.551f, 0.0f,  //79
                1,0,0,
                x*-0.854f, x*0.449f, 0.0f,  //78
                1,0,0,
                x*-0.829f, x*0.378f, 0.0f,  //75
                1,0,0,
                x*-0.794f, x*0.303f, 0.0f,  //74
                1,0,0,
                x*-0.755f, x*0.238f, 0.0f,  //71
                1,0,0,
                x*-0.687f, x*0.099f, 0.0f,  //69
                1,0,0,
                x*-0.595f, x*-0.043f, 0.0f,  // 36
                1,0,0,


            };
            triangleCenter1 = new vec3(0, 0, 0);

            float[] xyzAxesVertices = {
		        //x
		        0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        100.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        //y
	            0.0f, 0.0f, 0.0f,
                0.0f,1.0f, 0.0f, 
		        0.0f, 100.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 
		        //z
	            0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,  
		        0.0f, 0.0f, 100.0f,
                0.0f, 0.0f, 1.0f,  
            };

            

            triangleBufferID = GPU.GenerateBuffer(triangleVertices);
            bird = GPU.GenerateBuffer(triangleVertices1);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(0, 0, 25), // Camera is at (0,5,5), in World Space
                        new vec3(0, 0, 0), // and looks at the origin
                        new vec3(0, 1, 0)  // Head is up (set to 0,-1,0 to look upside-down)
                );
            // Model Matrix Initialization
            ModelMatrix = new mat4(1);
            ModelMatrix1 = new mat4(1);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
            
            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }
        float i = -47;
        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            #region XYZ axis

            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, xyzAxesBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, new mat4(1).to_array()); // Identity

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
             
            Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            if (i > 207)
            {
                Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, triangleBufferID);
                Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());
            }
            else
            {
                Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, bird);
                Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix1.to_array());

            }
            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_TRIANGLE_STRIP, 0, 5);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 5, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 8, 8);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 16, 10);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 26, 12);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 38, 9);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 47, 7);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 54, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 57, 7);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 64, 13);


            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            ////////////////////////////

            if (i < 207)
            {
                Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, triangleBufferID);
                Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());
            }
            else
            {
                Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, bird);
                Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix1.to_array());

            }

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));

            Gl.glDrawArrays(Gl.GL_TRIANGLE_STRIP, 0, 5);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 5, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 8, 8);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 16, 10);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 26, 12);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 38, 9);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 47, 7);
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 54, 3);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 57, 7);
            Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 64, 13);


            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
        }
        

        public void Update()
        {

            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/1000.0f;

            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), triangleCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, -1, 0)));
            transformations.Add(glm.translate(new mat4(1), triangleCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

           ModelMatrix =  MathHelper.MultiplyMatrices(transformations);


            List<mat4> transformation = new List<mat4>();
            transformation.Add(glm.translate(new mat4(1), triangleCenter));
            transformation.Add(glm.rotate(rotationAngle, new vec3(0, -1, 0)));
            transformation.Add(glm.translate(new mat4(1), triangleCenter));
            transformation.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix1 = MathHelper.MultiplyMatrices(transformation);

            i++;
            if (i >= 414)
                i = 0;
            timer.Reset();
            timer.Start();
        }
        
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
