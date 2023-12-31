﻿using System;
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

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        
        uint triangleBufferID;
        uint xyzAxesBufferID;

        // 3D Drawing
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        
        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        const float rotationSpeed = 1f;
        float rotationAngle = 0;

        public float translationX = 0, 
                     translationY = 0, 
                     translationZ = 0;

        Stopwatch timer = Stopwatch.StartNew();

        vec3 triangleCenter;

        public void Initialize() {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

            Gl.glClearColor(0.44f, 0.44f, 0.44f, 1);
            
            float[] triangleVertices = { 
                // P1
		        10 * -0.808f, 10 * 0.720f, 0, // 0
                 0, 0, 0,
                10 * -0.718f, 10 * 0.623f, 0, // 1
                 0, 0, 0,
                10 * -0.752f, 10 * 0.586f, 0, // 2
                 0, 0, 0,
                10 * -0.865f, 10 * 0.686f, 0, // 3
                0, 0, 0,

                // P2
                10 * -0.865f, 10 * 0.686f, 0, // 4
                 0, 0, 0,
                10 * -0.752f, 10 * 0.586f, 0, // 5
                0, 0, 0,
                10 * -0.758f, 10 * 0.519f, 0, // 6
                0, 0, 0,
                10 * -0.899f, 10 * 0.623f, 0, // 7
                0, 0, 0,

                // P3
                10 * -0.899f, 10 * 0.623f, 0, // 8
                0, 0, 0,
                10 * -0.758f, 10 * 0.519f, 0, // 9
                0, 0, 0,
                10 * -0.741f, 10 * 0.431f, 0, // 10
                0, 0, 0,
                10 * -0.910f, 10 * 0.498f, 0, // 11
                0, 0, 0,

                // P4
                10 * -0.910f, 10 * 0.498f, 0, // 12
                0, 0, 0,
                10 * -0.741f, 10 * 0.431f, 0, // 13
                0, 0, 0,
                10 * -0.713f, 10 * 0.377f, 0, // 14
                0, 0, 0,
                10 * -0.910f, 10 * 0.444f, 0, // 15
                0, 0, 0,

                // P5
                10 * -0.910f, 10 * 0.444f, 0, // 16
                0, 0, 0,
                10 * -0.713f, 10 * 0.377f, 0, // 17
                0, 0, 0,
                10 * -0.662f, 10 * 0.305f, 0, // 18
                0, 0, 0,
                10 * -0.876f, 10 * 0.356f, 0, // 19
                0, 0, 0,

                // P6
                10 * -0.718f, 10 * 0.623f, 0, // 20
                0, 0, 0,
                10 * -0.752f, 10 * 0.586f, 0, // 21
                0, 0, 0,
                10 * -0.465f, 10 * 0.285f, 0, // 22
                0, 0, 0,
                10 * -0.499f, 10 * 0.377f, 0, // 23
                0, 0, 0,

                // P7
                10 * -0.839f, 10 * 0.276f, 0, // 24
                0, 0, 0,
                10 * -0.662f, 10 * 0.305f, 0, // 25
                0, 0, 0,
                10 * -0.634f, 10 * 0.264f, 0, // 26
                0, 0, 0,
                10 * -0.808f, 10 * 0.222f, 0, // 27
                0, 0, 0,

                // P8
                10 * -0.808f, 10 * 0.222f, 0, // 28
                0, 0, 0,
                10 * -0.634f, 10 * 0.264f, 0, // 29
                0, 0, 0,
                10 * -0.594f, 10 * 0.234f, 0, // 30
                0, 0, 0,
                10 * -0.775f, 10 * 0.163f, 0, // 31
                0, 0, 0,

                // P9
                10 * -0.775f, 10 * 0.163f, 0, // 32
                0, 0, 0,
                10 * -0.594f, 10 * 0.234f, 0, // 33
                0, 0, 0,
                10 * -0.572f, 10 * 0.213f, 0, // 34
                0, 0, 0,
                10 * -0.741f, 10 * 0.105f, 0, // 35
                0, 0, 0,

                // P10
                10 * 0.172f, 10 * 0.347f, 0, // 36
                0, 0, 0,
                10 * 0.228f, 10 * 0.335f, 0, // 37
                0, 0, 0,
                10 * 0.228f, 10 * 0.314f, 0, // 38
                0, 0, 0,
                10 * 0.149f, 10 * 0.326f, 0, // 39
                0, 0, 0,

                // P11
                10 * 0.149f, 10 * 0.326f, 0, // 40
                0, 0, 0,
                10 * 0.228f, 10 * 0.314f, 0, // 41
                0, 0, 0,
                10 * 0.211f, 10 * 0.272f, 0, // 42
                0, 0, 0,
                10 * 0.082f, 10 * 0.268f, 0, // 43
                0, 0, 0,

                // P12
                10 * 0.082f, 10 * 0.268f, 0, // 44
                0, 0, 0,
                10 * 0.149f, 10 * 0.326f, 0, // 45
                0, 0, 0,
                10 * 0.093f, 10 * 0.347f, 0, // 46
                0, 0, 0,
                10 * 0.025f, 10 * 0.339f, 0, // 47
                0, 0, 0,

                // P13
                10 * 0.082f, 10 * 0.268f, 0, // 48
                0, 0, 0,
                10 * 0.025f, 10 * 0.339f, 0, // 49
                0, 0, 0,
                10 * -0.087f, 10 * 0.301f, 0, // 50
                0, 0, 0,
                10 * -0.099f, 10 * 0.276f, 0, // 51
                0, 0, 0,
                10 * -0.082f, 10 * 0.280f, 0, // 52
                0, 0, 0,

                // P14
                10 * 0.211f, 10 * 0.272f, 0, // 53
                0, 0, 0,
                10 * 0.335f, 10 * 0.272f, 0, // 54
                0, 0, 0,
                10 * 0.363f, 10 * 0.218f, 0, // 55
                0, 0, 0,
                10 * 0.346f, 10 * 0.213f, 0, // 56
                0, 0, 0,
                10 * 0.330f, 10 * 0.201f, 0, // 57
                0, 0, 0,

                // P15
                10 * 0.211f, 10 * 0.272f, 0, // 58
                0, 0, 0,
                10 * 0.268f, 10 * 0.155f, 0, // 59
                0, 0, 0,
                10 * 0.239f, 10 * 0.126f, 0, // 60
                0, 0, 0,
                10 * 0.087f, 10 * 0.176f, 0, // 61
                0, 0, 0,

                // P16
                10 * -0.099f, 10 * 0.276f, 0, // 62
                0, 0, 0,
                10 * -0.115f, 10 * 0.264f, 0, // 63
                0, 0, 0,
                10 * -0.110f, 10 * 0.192f, 0, // 64
                0, 0, 0,
                10 * -0.099f, 10 * 0.192f, 0, // 65
                0, 0, 0,

                // P17
                10 * -0.091f, 10 * 0.042f, 0, // 66
                0, 0, 0,
                10 * -0.099f, 10 * 0.192f, 0, // 67
                0, 0, 0,
                10 * -0.110f, 10 * 0.193f, 0, // 68
                0, 0, 0,
                10 * -0.115f, 10 * 0.155f, 0, // 69
                0, 0, 0,
                -0.152f, 0.132f, 0, // 70
                0, 0, 0,
                -0.211f, 0.117f, 0, // 71
                0, 0, 0,

                // P18
                10 *  0.087f, 10 * 0.176f, 0, // 72
                0, 0, 0,
                10 *  0.132f, 10 * 0.031f, 0, // 73
                0, 0, 0,
                10 *  0.031f, 10 * 0.054f, 0, // 74
                0, 0, 0,
                10 * -0.008f, 10 * 0.109f, 0, // 75
                0, 0, 0,

                // P19
                10 * 0.132f, 10 * 0.031f, 0, // 76
                1, 1, 1,
                10 * 0.031f, 10 * 0.054f, 0, // 77
                1, 1, 1,
                10 * 0.155f,-10 * 0.046f, 0, // 78
                1, 1, 1,
                10 * 0.172f,-10 * 0.008f, 0, // 79
                1, 1, 1,

                // P20
                10 * 0.031f, 10 * 0.054f, 0, // 80
                1, 1, 1,
                10 * 0.155f,-10 * 0.046f, 0, // 81
                1, 1, 1,
                10 * 0.144f,-10 * 0.071f, 0, // 82
                1, 1, 1,
                -10 * 0.093f, 10 * 0.042f, 0, // 83
                1, 1, 1,

                // P21
                -10 * 0.093f, 10 * 0.042f, 0, // 84
                1, 1, 1,
                10 * 0.144f,-10 * 0.071f, 0, // 85
                1, 1, 1,
                10 * 0.115f,-10 * 0.134f, 0, // 86
                1, 1, 1,
                10 * 0.014f,-10 * 0.184f, 0, // 87
                1, 1, 1,

                // P22
                -10 * 0.211f, 10 * 0.117f, 0, // 88
                1, 0, 0,
                -10 * 0.245f, 10 * 0.117f, 0, // 89
                1, 0, 0,
                -10 * 0.392f, 10 * 0.013f, 0, // 90
                1, 0, 0,
                -10 * 0.262f,-10 * 0.088f, 0, // 91
                1, 0, 0,

                // P23
                -10 * 0.392f, 10 * 0.013f, 0, // 92
                1, 0, 0,
                -10 * 0.583f,-10 * 0.138f, 0, // 93
                1, 0, 0,
                -10 * 0.589f,-10 * 0.155f, 0, // 94
                1, 0, 0,
                -10 * 0.414f,-10 * 0.146f, 0, // 95
                1, 0, 0,

                // P24
                -10 * 0.262f,-10 * 0.088f, 0, // 96
                1, 1, 1,
                -10 * 0.138f,-10 * 0.134f, 0, // 97
                1, 1, 1,
                -10 * 0.104f,-10 * 0.176f, 0, // 98
                1, 1, 1,
                -10 * 0.228f,-10 * 0.172f, 0, // 99
                1, 1, 1,

                // P25
                -10 * 0.104f,-10 * 0.176f, 0, // 100
                1, 1, 1,
                10 * 0.003f,-10 * 0.272f, 0, // 101
                1, 1, 1,
                10 * 0.031f,-10 * 0.251f, 0, // 102
                1, 1, 1,
                10 * 0.014f,-10 * 0.184f, 0, // 103
                1, 1, 1,

                // P26
                -10 * 0.589f,-10 * 0.155f, 0, // 104
                1, 0, 0,
                -10 * 0.634f,-10 * 0.289f, 0, // 105
                1, 0, 0,
                -10 * 0.634f,-10 * 0.343f, 0, // 106
                1, 0, 0,
                -10 * 0.487f,-10 * 0.285f, 0, // 107
                1, 0, 0,

                // P27
                -10 * 0.634f,-10 * 0.343f, 0, // 108
                0, 1, 0,
                -10 * 0.487f,-10 * 0.285f, 0, // 109
                0, 1, 0,
                -10 * 0.527f,-10 * 0.494f, 0, // 110
                0, 1, 0,
                -10 * 0.634f,-10 * 0.397f, 0, // 111
                0, 1, 0,

                // P28
                -10 * 0.634f,-10 * 0.397f, 0, // 112
                0, 1, 0,
                -10 * 0.527f,-10 * 0.494f, 0, // 113
                0, 1, 0,
                -10 * 0.611f,-10 * 0.611f, 0, // 114
                0, 1, 0,
                -10 * 0.639f,-10 * 0.552f, 0, // 115
                0, 1, 0,

                // P29
                -10 * 0.527f,-10 * 0.494f, 0, // 116
                0, 1, 0,
                -10 * 0.470f,-10 * 0.703f, 0, // 117
                0, 1, 0,
                -10 * 0.504f,-10 * 0.904f, 0, // 118
                0, 1, 0,
                -10 * 0.532f,-10 * 0.841f, 0, // 119
                0, 1, 0,

                // P30
                -10 * 0.470f,-10 * 0.703f, 0, // 120
                0, 1, 0,
                -10 * 0.431f,-10 * 0.820f, 0, // 121
                0, 1, 0,
                -10 * 0.408f,-10 * 0.799f, 0, // 122
                0, 1, 0,
                -10 * 0.386f,-10 * 0.720f, 0, // 123
                0, 1, 0,
                -10 * 0.454f,-10 * 0.615f, 0, // 124
                0, 1, 0,

                // P31
                -10 * 0.386f,-10 * 0.720f, 0, // 125
                0, 1, 0,
                -10 * 0.454f,-10 * 0.615f, 0, // 126
                0, 1, 0,
                -10 * 0.403f,-10 * 0.473f, 0, // 127
                0, 1, 0,
                -10 * 0.386f,-10 * 0.690f, 0, // 128
                0, 1, 0,
                -10 * 0.380f,-10 * 0.707f, 0, // 129
                0, 1, 0,

                // P32
                -10 * 0.469f, 10 * 0.289f, 0, // 130
                0, 0, 0,
                -10 * 0.335f, 10 * 0.205f, 0, // 131
                0, 0, 0,
                -10 * 0.285f, 10 * 0.159f, 0, // 132
                0, 0, 0,
                -10 * 0.442f, 10 * 0.180f, 0, // 133
                0, 0, 0,

                // P33
                -10 * 0.285f, 10 * 0.159f, 0, // 134
                0, 0, 0,
                -10 * 0.442f, 10 * 0.180f, 0, // 135
                0, 0, 0,
                -10 * 0.285f, 10 * 0.087f, 0, // 136
                0, 0, 0,
                -10 * 0.245f, 10 * 0.117f, 0, // 137
                0, 0, 0,

                // P34
                -10 * 0.228f,-10 * 0.172f, 0, // 138
                0, 1, 0,
                -10 * 0.380f,-10 * 0.256f, 0, // 139
                0, 1, 0,
                -10 * 0.329f,-10 * 0.414f, 0, // 140
                0, 1, 0,
                -10 * 0.313f,-10 * 0.406f, 0, // 141
                0, 1, 0,

                 // P35
                -10 * 0.091f, 10 * 0.043f, 0, // 142
                1, 1, 1,
                -10 * 0.262f,-10 * 0.088f, 0, // 143
                1, 1, 1,
                -10 * 0.211f, 10 * 0.117f, 0, // 144
                1, 1, 1,
                -10 * 0.209f, 10 * 0.117f, 0, // 145
                1, 1, 1,

                 
                 // T1
                -10 * 0.752f, 10 * 0.586f, 0, // 146
                0, 0, 0,
                -10 * 0.758f, 10 * 0.519f, 0, // 147
                0, 0, 0,
                -10 * 0.465f, 10 * 0.285f, 0, // 148
                0, 0, 0,

                 // T2
                -10 * 0.465f, 10 * 0.285f, 0, // 149
                0, 0, 0,
                -10 * 0.758f, 10 * 0.519f, 0, // 150
                0, 0, 0,
                -10 * 0.741f, 10 * 0.431f, 0, // 151
                0, 0, 0,

                 // T3
                -10 * 0.465f, 10 * 0.285f, 0, // 152
                0, 0, 0,
                -10 * 0.741f, 10 * 0.431f, 0, // 153
                0, 0, 0,
                -10 * 0.713f, 10 * 0.377f, 0, // 154
                0, 0, 0,

                 // T4
                -10 * 0.465f, 10 * 0.285f, 0, // 155
                0, 0, 0,
                -10 * 0.713f, 10 * 0.377f, 0, // 156
                0, 0, 0,
                -10 * 0.662f, 10 * 0.305f, 0, // 157
                0, 0, 0,

                 // T5
                -10 * 0.465f, 10 * 0.285f, 0, // 158
                0, 0, 0,
                -10 * 0.662f, 10 * 0.305f, 0, // 159
                0, 0, 0,
                -10 * 0.634f, 10 * 0.264f, 0, // 160
                0, 0, 0,

                 // T6
                -10 * 0.465f, 10 * 0.285f, 0, // 161
                0, 0, 0,
                -10 * 0.634f, 10 * 0.264f, 0, // 162
                0, 0, 0,
                -10 * 0.594f, 10 * 0.234f, 0, // 163
                0, 0, 0,

                 // T7
                -10 * 0.465f, 10 * 0.285f, 0, // 164
                0, 0, 0,
                -10 * 0.594f, 10 * 0.234f, 0, // 165
                0, 0, 0,
                -10 * 0.572f, 10 * 0.213f, 0, // 166
                0, 0, 0,

                 // T8
                -10 * 0.465f, 10 * 0.285f, 0, // 167
                0, 0, 0,
                -10 * 0.572f, 10 * 0.213f, 0, // 168
                0, 0, 0,
                -10 * 0.515f, 10 * 0.160f, 0, // 169
                0, 0, 0,

                 // T9
                -10 * 0.465f, 10 * 0.285f, 0, // 170
                0, 0, 0,
                -10 * 0.499f, 10 * 0.375f, 0, // 171
                0, 0, 0,
                -10 * 0.341f, 10 * 0.209f, 0, // 172
                0, 0, 0,

                 // T10
                -10 * 0.465f, 10 *  0.285f, 0, // 173
                0, 0, 0,
                -10 * 0.515f, 10 * 0.160f, 0, // 174
                0, 0, 0,
                -10 * 0.442f, 10 * 0.180f, 0, // 175
                0, 0, 0,

                 // T11
                -10 * 0.515f, 10 * 0.160f, 0, // 176
                 1, 0, 0,
                -10 * 0.442f, 10 * 0.180f, 0, // 177
                 1, 0, 0,
                -10 * 0.452f, 10 * 0.092f, 0, // 178
                 1, 0, 0,

                 // T12
                -10 * 0.442f, 10 * 0.180f, 0, // 179
                1, 0, 0,
                -10 * 0.452f, 10 * 0.092f, 0, // 180
                1, 0, 0,
                -10 * 0.285f, 10 * 0.087f, 0, // 181
                1, 0, 0,

                 // T13
                -10 * 0.452f, 10 * 0.092f, 0, // 182
                1, 0, 0,
                -10 * 0.285f, 10 * 0.087f, 0, // 183
                1, 0, 0,
                -10 * 0.392f, 10 * 0.013f, 0, // 184
                1, 0, 0,

                 // T14
                -10 * 0.452f, 10 * 0.092f, 0, // 185
                1, 0, 0,
                -10 * 0.392f, 10 * 0.013f, 0, // 186
                1, 0, 0,
                -10 * 0.583f,-10 * 0.138f, 0, // 187
                1, 0, 0,

                 // T15
                -10 * 0.452f, 10 * 0.092f, 0, // 188
                1, 0, 0,
                -10 * 0.671f,-10 * 0.005f, 0, // 189
                1, 0, 0,
                -10 * 0.583f,-10 * 0.138f, 0, // 190
                1, 0, 0,

                 // T16
                -10 * 0.452f, 10 * 0.092f, 0, // 191
                1, 0, 0,
                -10 * 0.671f,-10 * 0.005f, 0, // 192
                1, 0, 0,
                -10 * 0.515f, 10 * 0.160f, 0, // 193
                1, 0, 0,

                 // T17
                -10 * 0.741f, 10 * 0.105f, 0, // 194
                1, 0, 0,
                -10 * 0.515f, 10 * 0.160f, 0, // 195
                1, 0, 0,
                -10 * 0.671f,-10 * 0.005f, 0, // 196
                1, 0, 0,

                 // T18
                -10 * 0.741f, 10 * 0.105f, 0, // 197
                0, 0, 0,
                -10 * 0.515f, 10 * 0.160f, 0, // 198
                0, 0, 0,
                -10 * 0.572f, 10 * 0.213f, 0, // 199
                0, 0, 0,

                 // T19
                -10 * 0.662f, 10 * 0.305f, 0, // 200
                0, 0, 0,
                -10 * 0.876f, 10 * 0.356f, 0, // 201
                0, 0, 0,
                -10 * 0.839f, 10 * 0.276f, 0, // 202
                0, 0, 0,

                 // T20
                -10 * 0.470f,-10 * 0.703f, 0, // 203
                0, 1, 0,
                -10 * 0.504f,-10 * 0.904f, 0, // 204
                0, 1, 0,
                -10 * 0.431f,-10 * 0.820f, 0, // 205
                0, 1, 0,

                 // T21
                -10 * 0.611f,-10 * 0.611f, 0, // 206
                0, 1, 0,
                -10 * 0.527f,-10 * 0.494f, 0, // 207
                0, 1, 0,
                -10 * 0.532f,-10 * 0.841f, 0, // 208
                0, 1, 0,

                 // T22
                -10 * 0.527f,-10 * 0.494f, 0, // 209
                0, 1, 0,
                -10 * 0.454f,-10 * 0.615f, 0, // 210
                0, 1, 0,
                -10 * 0.464f,-10 * 0.460f, 0, // 211
                0, 1, 0,

                 // T23
                -10 * 0.527f,-10 * 0.494f, 0, // 212
                0, 1, 0,
                -10 * 0.464f,-10 * 0.460f, 0, // 213
                0, 1, 0,
                -10 * 0.487f,-10 * 0.285f, 0, // 214
                0, 1, 0,

                 // T24
                -10 * 0.487f,-10 * 0.285f, 0, // 215
                0, 1, 0,
                -10 * 0.464f,-10 * 0.460f, 0, // 216
                0, 1, 0,
                -10 * 0.403f,-10 * 0.473f, 0, // 217
                0, 1, 0,

                 // T25
                -10 * 0.464f,-10 * 0.460f, 0, // 218
                0, 1, 0,
                -10 * 0.403f,-10 * 0.473f, 0, // 219
                0, 1, 0,
                -10 * 0.454f,-10 * 0.615f, 0, // 220
                0, 1, 0,

                 // T26
                -10 * 0.527f,-10 * 0.494f, 0, // 221
                0, 1, 0,
                -10 * 0.454f,-10 * 0.615f, 0, // 222
                0, 1, 0,
                -10 * 0.470f,-10 * 0.703f, 0, // 223
                0, 1, 0,

                 // T27
                -10 * 0.403f,-10 * 0.473f, 0, // 224
                0, 1, 0,
                -10 * 0.328f,-10 * 0.632f, 0, // 225
                0, 1, 0,
                -10 * 0.386f,-10 * 0.690f, 0, // 226
                0, 1, 0,

                 // T28
                -10 * 0.403f,-10 * 0.473f, 0, // 227
                0, 1, 0,
                -10 * 0.328f,-10 * 0.632f, 0, // 228
                0, 1, 0,
                -10 * 0.329f,-10 * 0.414f, 0, // 229
                0, 1, 0,

                // T29
                -10 * 0.403f,-10 * 0.473f, 0, // 230
                0, 1, 0,
                -10 * 0.329f,-10 * 0.414f, 0, // 231
                0, 1, 0,
                -10 * 0.414f,-10 * 0.146f, 0, // 232
                0, 1, 0,

                // T30
                -10 * 0.403f,-10 * 0.473f, 0, // 233
                0, 1, 0,
                -10 * 0.414f,-10 * 0.146f, 0, // 234
                0, 1, 0,
                -10 * 0.487f,-10 * 0.285f, 0, // 235
                0, 1, 0,

                // T31
                -10 * 0.414f,-10 * 0.146f, 0, // 236
                1, 0, 0,
                -10 * 0.487f,-10 * 0.285f, 0, // 237
                1, 0, 0,
                -10 * 0.589f,-10 * 0.155f, 0, // 238
                1, 0, 0,

                // T32
                -10 * 0.414f,-10 * 0.146f, 0, // 239
                1, 1, 1,
                -10 * 0.228f,-10 * 0.172f, 0, // 240
                1, 1, 1,
                -10 * 0.378f,-10 * 0.258f, 0, // 241
                1, 1, 1,

                // T33
                -10 * 0.414f,-10 * 0.146f, 0, // 242
                1, 1, 1,
                -10 * 0.228f,-10 * 0.172f, 0, // 243
                1, 1, 1,
                -10 * 0.262f,-10 * 0.088f, 0, // 244
                1, 1, 1,

                // T34
                -10 * 0.414f,-10 * 0.146f, 0, // 245
                1, 0, 0,
                -10 * 0.262f,-10 * 0.088f, 0, // 246
                1, 0, 0,
                -10 * 0.392f, 10 * 0.013f, 0, // 247
                1, 0, 0,

                // T35
                -10 * 0.228f,-10 * 0.172f, 0, // 248
                0, 1, 0,
                -10 * 0.138f,-10 * 0.335f, 0, // 249
                0, 1, 0,
                -10 * 0.313f,-10 * 0.406f, 0, // 250
                0, 1, 0,

                // T36
                -10 * 0.228f,-10 * 0.172f, 0, // 251
                0, 1, 0,
                -10 * 0.138f,-10 * 0.335f, 0, // 252
                0, 1, 0,
                10 * 0.003f,-10 * 0.272f, 0, // 253
                0, 1, 0,

                // T37
                -10 * 0.228f,-10 * 0.172f, 0, // 254
                1, 1, 1,
                10 * 0.003f,-10 * 0.272f, 0, // 255
                1, 1, 1,
                -10 * 0.104f,-10 * 0.176f, 0, // 256
                1, 1, 1,

                // T38
                -10 * 0.104f,-10 * 0.176f, 0, // 257
                1, 1, 1,
                -10 * 0.138f,-10 * 0.134f, 0, // 258
                1, 1, 1,
                10 * 0.014f,-10 * 0.184f, 0, // 259
                1, 1, 1,

                // T39
                10 * 0.014f,-10 * 0.184f, 0, // 260
                1, 1, 1,
                10 * 0.115f,-10 * 0.134f, 0, // 261
                1, 1, 1,
                10 * 0.031f,-10 * 0.251f, 0, // 262
                1, 1, 1,

                // T40
                10 * 0.014f,-10 * 0.184f, 0, // 263
                1, 1, 1,
                -10 * 0.138f,-10 * 0.134f, 0, // 264
                1, 1, 1,
                -10 * 0.093f, 10 * 0.042f, 0, // 265
                1, 1, 1,

                // T41
                -10 * 0.138f,-10 * 0.134f, 0, // 266
                1, 1, 1,
                -10 * 0.093f, 10 * 0.042f, 0, // 267
                1, 1, 1,
                -10 * 0.262f,-10 * 0.088f, 0, // 268
                1, 1, 1,

                // T42
                -10 * 0.093f, 10 * 0.042f, 0, // 269
                0, 0, 0,
                10 * 0.031f, 10 * 0.054f, 0, // 270
                0, 0, 0,
                -10 * 0.008f, 10 * 0.109f, 0, // 271
                0, 0, 0,

                // T43
                -10 * 0.095f, 10 * 0.040f, 0, // 272
                0, 0, 0,
                -10 * 0.008f, 10 * 0.109f, 0, // 273
                0, 0, 0,
                -10 * 0.049f, 10 * 0.180f, 0, // 274
                0, 0, 0,

                // T44
                -10 * 0.093f, 10 * 0.042f, 0, // 275
                0, 0, 0,
                -10 * 0.049f, 10 * 0.180f, 0, // 276
                0, 0, 0,
                -10 * 0.093f, 10 * 0.042f, 0, // 277
                0, 0, 0,

                // T45
                -10 * 0.049f, 10 * 0.180f, 0, // 278
                0, 0, 0,
                -10 * 0.094f, 10 * 0.042f, 0, // 279
                0, 0, 0,
                -10 * 0.099f, 10 * 0.276f, 0, // 280
                0, 0, 0,

                // T46
                -10 * 0.008f,10 *  0.109f, 0, // 281
                0, 0, 0,
                -10 * 0.049f, 10 * 0.180f, 0, // 282
                0, 0, 0,
                10 * 0.087f, 10 * 0.176f, 0, // 283
                0, 0, 0,

                // T47
                -10 * 0.049f, 10 * 0.180f, 0, // 284
                0, 0, 0,
                10 * 0.087f, 10 * 0.176f, 0, // 285
                0, 0, 0,
                -10 * 0.099f, 10 * 0.276f, 0, // 286
                0, 0, 0,

                // T48
                10 * 0.087f, 10 * 0.176f, 0, // 287
                0, 0, 0,
                -10 * 0.099f, 10 * 0.276f, 0, // 288
                0, 0, 0,
                10 * 0.082f, 10 * 0.268f, 0, // 289
                0, 0, 0,

                // T49
                10 * 0.087f, 10 * 0.176f, 0, // 290
                0, 0, 0,
                10 * 0.082f, 10 * 0.268f, 0, // 291
                0, 0, 0,
                10 * 0.211f, 10 * 0.272f, 0, // 292
                0, 0, 0,

                // T50
                10 * 0.087f, 10 * 0.176f, 0, // 293
                0, 0, 0,
                10 * 0.239f, 10 * 0.126f, 0, // 294
                0, 0, 0,
                10 * 0.132f, 10 * 0.031f, 0, // 295
                0, 0, 0,

                // T51
                10 * 0.239f, 10 * 0.126f, 0, // 296
                0, 0, 0,
                10 * 0.132f, 10 * 0.031f, 0, // 297
                0, 0, 0,
                10 * 0.172f,-10 * 0.008f, 0, // 298
                0, 0, 0,

                // T52
                10 * 0.211f, 10 * 0.272f, 0, // 299
                0, 0, 0,
                10 * 0.330f, 10 * 0.201f, 0, // 300
                0, 0, 0,
                10 * 0.268f, 10 * 0.155f, 0, // 301
                0, 0, 0,

                // T53
                10 * 0.211f, 10 * 0.272f, 0, // 302
                0, 0, 0,
                10 * 0.335f,10 *  0.272f, 0, // 303
                0, 0, 0,
                10 * 0.228f, 10 * 0.314f, 0, // 304
                0, 0, 0,

                // T54
                10 * 0.335f, 10 * 0.272f, 0, // 305
                0, 0, 0,
                10 * 0.228f, 10 * 0.314f, 0, // 306
                0, 0, 0,
                10 * 0.255f, 10 * 0.313f, 0, // 307
                0, 0, 0,

                // T55
                10 * 0.228f, 10 * 0.314f, 0, // 308
                0, 0, 0,
                10 * 0.255f, 10 * 0.313f, 0, // 309
                0, 0, 0,
                10 * 0.228f, 10 * 0.335f, 0, // 310
                0, 0, 0,

                // T56
                10 * 0.172f, 10 * 0.347f, 0, // 311
                0, 0, 0,
                10 * 0.149f, 10 * 0.326f, 0, // 312
                0, 0, 0,
                10 * 0.093f, 10 * 0.347f, 0, // 313
                0, 0, 0,

                // T57
                10 * 0.335f, 10 * 0.272f, 0, // 314
                0, 0, 0,
                10 * 0.363f, 10 * 0.218f, 0, // 315
                0, 0, 0,
                10 * 0.425f, 10 * 0.264f, 0, // 316
                0, 0, 0,

                // T58
                10 * 0.363f, 10 * 0.218f, 0, // 317
                0, 0, 0,
                10 * 0.425f, 10 * 0.264f, 0, // 318
                0, 0, 0,
                10 * 0.469f, 10 * 0.218f, 0, // 319
                0, 0, 0,

                // T59
                10 * 0.425f, 10 * 0.264f, 0, // 320
                0, 0, 0,
                10 * 0.469f, 10 * 0.218f, 0, // 321
                0, 0, 0,
                10 * 0.925f, 10 * 0.235f, 0,  // 322
                0, 0, 0,

                }; // Triangle Center = (10, 7, -5)
            
            triangleCenter = new vec3(0, 0, 0);

            float[] xyzAxesVertices = {
		        // X
		        0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 
		        100.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, 

		        // Y
	            0.0f, 0.0f, 0.0f,
                0.0f,1.0f, 0.0f, 
		        0.0f, 100.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 

		        // Z
	            0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,  
		        0.0f, 0.0f, 100.0f,
                0.0f, 0.0f, 1.0f,  
            };


            triangleBufferID = GPU.GenerateBuffer(triangleVertices);
            xyzAxesBufferID = GPU.GenerateBuffer(xyzAxesVertices);

            // View Matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(0, 0, 25), // Camera is at (0, 5, 5), in World Space
                        new vec3(0, 0, 0),  // and looks at the origin
                        new vec3(0, 1, 0)   // Head is up (set to 0, -1, 0 to look upside-down)
                    );

            // Model Matrix Initialization
            ModelMatrix = new mat4(1);

            // ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
            
            // Our MVP matrix which is a multiplication of our 3 matrices 
            sh.UseShader();


            // Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");

            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

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
             
            //Gl.glDrawArrays(Gl.GL_LINES, 0, 6);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);

            #endregion

            #region Animated Triangle
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, triangleBufferID);
            Gl.glUniformMatrix4fv(ShaderModelMatrixID, 1, Gl.GL_FALSE, ModelMatrix.to_array());

            Gl.glEnableVertexAttribArray(0);
            Gl.glEnableVertexAttribArray(1);

            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)0);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            
            // Draw Poligons 1 : 35
            HashSet<int> poligons5 = new HashSet<int>();
            HashSet<int> poligons6 = new HashSet<int>();
            poligons5.Add(48); 
            poligons5.Add(53); 
            poligons5.Add(120);
            poligons5.Add(125);

            poligons6.Add(66);

            for(int P = 0, count = 4; P < 146; P += count) {
                count = 4;        // Default
                if(poligons5.Contains(P)) count = 5;
                if(poligons6.Contains(P)) count = 6;

                Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, P, count);  
                
            }

            // Draw Triangles 1 : 59
            Gl.glDrawArrays(Gl.GL_TRIANGLES, 146, 180);

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            #endregion
        }
        

        public void Update()
        {

            timer.Stop();
            var deltaTime = timer.ElapsedMilliseconds/1000.0f;

            rotationAngle += deltaTime * rotationSpeed;

            List<mat4> transformations = new List<mat4>();
            transformations.Add(glm.translate(new mat4(1), -1 * triangleCenter));
            transformations.Add(glm.rotate(rotationAngle, new vec3(0, 1, 0)));
            transformations.Add(glm.translate(new mat4(1),  triangleCenter));
            transformations.Add(glm.translate(new mat4(1), new vec3(translationX, translationY, translationZ)));

            ModelMatrix =  MathHelper.MultiplyMatrices(transformations);
            
            timer.Reset();
            timer.Start();
        }
        
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
