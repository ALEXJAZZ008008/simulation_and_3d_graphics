﻿using Labs.Utility;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;

namespace Labs.ACW
{
    public class ACWWindow : GameWindow
    {
        public ACWWindow()
            : base(
                800, // Width
                600, // Height
                GraphicsMode.Default,
                "Assessed Coursework",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible
                )
        {

        }

        private int[] mVBO_IDs = new int[9];
        private int[] mVAO_IDs = new int[5];
        private ShaderUtility mShader;
        private ModelUtility mBoxModelUtility;
        private Matrix4 mView, mEmitterBoxModel, mGridBox1Model, mGridBox2Model, mSphereOfDoomBoxModel, mWorld;

        protected override void OnLoad(EventArgs e)
        {
            int size;

            // Set some GL state
            GL.ClearColor(Color4.White);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            mShader = new ShaderUtility(@"ACW/Shaders/vPassThrough.vert", @"ACW/Shaders/fLighting.frag");
            GL.UseProgram(mShader.ShaderProgramID);

            int vPositionLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vPosition");
            int vNormalLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vNormal");
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            int uEyePositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");

            #region Light

            int uLightPositionLocation0 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].Position");
            int uAmbientLightLocation0 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].AmbientLight");
            int uDiffuseLightLocation0 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].DiffuseLight");
            int uSpecularLightLocation0 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].SpecularLight");
            int uLightPositionLocation1 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].Position");
            int uAmbientLightLocation1 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].AmbientLight");
            int uDiffuseLightLocation1 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].DiffuseLight");
            int uSpecularLightLocation1 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].SpecularLight");
            int uLightPositionLocation2 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].Position");
            int uAmbientLightLocation2 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].AmbientLight");
            int uDiffuseLightLocation2 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].DiffuseLight");
            int uSpecularLightLocation2 = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].SpecularLight");

            #endregion

            #region World

            GL.GenVertexArrays(mVAO_IDs.Length, mVAO_IDs);
            GL.GenBuffers(mVBO_IDs.Length, mVBO_IDs);

            float[] vertices = new float[] { 0, 0, 0, 0, 0, 0,
                                             0, 0, 0, 0, 0, 0,
                                             0, 0, 0, 0, 0, 0,
                                             0, 0, 0, 0, 0, 0 };

            GL.BindVertexArray(mVAO_IDs[0]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            #endregion

            #region EmitterBox

            GL.BindVertexArray(0);

            mBoxModelUtility = ModelUtility.LoadModel(@"Utility/Models/lab22model.sjg");

            GL.BindVertexArray(mVAO_IDs[1]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mBoxModelUtility.Vertices.Length * sizeof(float)), mBoxModelUtility.Vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[2]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mBoxModelUtility.Indices.Length * sizeof(float)), mBoxModelUtility.Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (mBoxModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);

            if (mBoxModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            #endregion

            #region GridBox1

            GL.BindVertexArray(0);

            mBoxModelUtility = ModelUtility.LoadModel(@"Utility/Models/lab22model.sjg");

            GL.BindVertexArray(mVAO_IDs[2]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[3]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mBoxModelUtility.Vertices.Length * sizeof(float)), mBoxModelUtility.Vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[4]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mBoxModelUtility.Indices.Length * sizeof(float)), mBoxModelUtility.Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (mBoxModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);

            if (mBoxModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            #endregion

            #region GridBox2

            GL.BindVertexArray(0);

            mBoxModelUtility = ModelUtility.LoadModel(@"Utility/Models/lab22model.sjg");

            GL.BindVertexArray(mVAO_IDs[3]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[5]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mBoxModelUtility.Vertices.Length * sizeof(float)), mBoxModelUtility.Vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[6]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mBoxModelUtility.Indices.Length * sizeof(float)), mBoxModelUtility.Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (mBoxModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);

            if (mBoxModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            #endregion

            #region SphereOfDoomBox

            GL.BindVertexArray(0);

            mBoxModelUtility = ModelUtility.LoadModel(@"Utility/Models/lab22model.sjg");

            GL.BindVertexArray(mVAO_IDs[4]);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[7]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mBoxModelUtility.Vertices.Length * sizeof(float)), mBoxModelUtility.Vertices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[8]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mBoxModelUtility.Indices.Length * sizeof(float)), mBoxModelUtility.Indices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (mBoxModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);

            if (mBoxModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            #endregion

            #region Translations

            mView = Matrix4.CreateTranslation(0, -1.5f, 0);
            GL.UniformMatrix4(uView, true, ref mView);

            mWorld = Matrix4.CreateTranslation(0, 1.5f, -5f);

            mEmitterBoxModel = Matrix4.CreateTranslation(0, 1.5f, -5f);
            mEmitterBoxModel = Matrix4.CreateScale(3);

            mGridBox1Model = Matrix4.CreateTranslation(0, 0.75f, -5f);
            mGridBox1Model = Matrix4.CreateScale(3);

            mGridBox2Model = Matrix4.CreateTranslation(0, -0.75f, -5f);
            mGridBox2Model = Matrix4.CreateScale(3);

            mSphereOfDoomBoxModel = Matrix4.CreateTranslation(0, -1.5f, -5f);
            mSphereOfDoomBoxModel = Matrix4.CreateScale(3);

            #endregion

            #region LightAndCamera

            Vector4 eyePosition = Vector4.Transform(new Vector4(0, 0, 0, 1), mView);
            GL.Uniform4(uEyePositionLocation, eyePosition);

            Vector4 lightPosition0 = Vector4.Transform(new Vector4(5, 5, -11f, 1), mView);
            GL.Uniform4(uLightPositionLocation0, lightPosition0);

            Vector4 lightPosition1 = Vector4.Transform(new Vector4(0, 5, -1f, 1), mView);
            GL.Uniform4(uLightPositionLocation1, lightPosition1);

            Vector4 lightPosition2 = Vector4.Transform(new Vector4(-5, 5, -11f, 1), mView);
            GL.Uniform4(uLightPositionLocation2, lightPosition2);

            Vector3 colour0 = new Vector3(1.0f, 1.0f, 1.0f);
            GL.Uniform3(uAmbientLightLocation0, colour0);
            GL.Uniform3(uDiffuseLightLocation0, colour0);
            GL.Uniform3(uSpecularLightLocation0, colour0);

            Vector3 colour1 = new Vector3(1.0f, 1.0f, 1.0f);
            GL.Uniform3(uAmbientLightLocation1, colour1);
            GL.Uniform3(uDiffuseLightLocation1, colour1);
            GL.Uniform3(uSpecularLightLocation1, colour1);

            Vector3 colour2 = new Vector3(1.0f, 1.0f, 1.0f);
            GL.Uniform3(uAmbientLightLocation2, colour2);
            GL.Uniform3(uDiffuseLightLocation2, colour2);
            GL.Uniform3(uSpecularLightLocation2, colour2);

            #endregion

            base.OnLoad(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                mView = mView * Matrix4.CreateTranslation(0.0f, 0.0f, 0.05f);
                int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
                GL.UniformMatrix4(uView, true, ref mView);
            }

            if (e.KeyChar == 'a')
            {
                mView = mView * Matrix4.CreateRotationY(-0.025f);
                int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
                GL.UniformMatrix4(uView, true, ref mView);
            }

            if (e.KeyChar == 's')
            {
                mView = mView * Matrix4.CreateTranslation(0.0f, 0.0f, -0.05f);
                int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
                GL.UniformMatrix4(uView, true, ref mView);
            }

            if (e.KeyChar == 'd')
            {
                mView = mView * Matrix4.CreateRotationY(0.025f);
                int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
                GL.UniformMatrix4(uView, true, ref mView);
            }

            if (e.KeyChar == 'z')
            {
                Vector3 t = mWorld.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mWorld = mWorld * inverseTranslation * Matrix4.CreateRotationY(-0.025f) * translation;
            }

            if (e.KeyChar == 'c')
            {
                Vector3 t = mWorld.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mWorld = mWorld * inverseTranslation * Matrix4.CreateRotationY(0.025f) * translation;
            }

            if (e.KeyChar == 'r')
            {
                Vector3 t = mWorld.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mWorld = mWorld * inverseTranslation * Matrix4.CreateRotationX(-0.025f) * translation;
            }

            if (e.KeyChar == 'f')
            {
                Vector3 t = mWorld.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mWorld = mWorld * inverseTranslation * Matrix4.CreateRotationX(0.025f) * translation;
            }

            base.OnKeyPress(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(this.ClientRectangle);

            if (mShader != null)
            {
                int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, 25);
                GL.UniformMatrix4(uProjectionLocation, true, ref projection);
            }

            base.OnResize(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
 	        base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            int uAmbientReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.AmbientReflectivity");
            int uDiffuseReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.DiffuseReflectivity");
            int uSpecularReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.SpecularReflectivity");
            int uShininessLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.Shininess");

            #region World

            GL.UniformMatrix4(uModel, true, ref mWorld);

            Vector3 worldAmbientReflectivity = new Vector3(0, 0, 0);
            GL.Uniform3(uAmbientReflectivityLocation, worldAmbientReflectivity);

            Vector3 worldDiffuseReflectivity = new Vector3(0, 0, 0);
            GL.Uniform3(uDiffuseReflectivityLocation, worldDiffuseReflectivity);

            Vector3 worldSpecularReflectivity = new Vector3(0, 0, 0);
            GL.Uniform3(uSpecularReflectivityLocation, worldSpecularReflectivity);

            float worldShininess = 0f;
            GL.Uniform1(uShininessLocation, worldShininess);

            

            GL.BindVertexArray(mVAO_IDs[0]);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            #endregion

            GL.CullFace(CullFaceMode.Front);

            #region EmitterBox

            Matrix4 m1 = mEmitterBoxModel * mWorld;
            uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref m1);

            Vector3 emitterBoxAmbientReflectivity = new Vector3(0.2125f, 0.1275f, 0.054f);
            GL.Uniform3(uAmbientReflectivityLocation, emitterBoxAmbientReflectivity);

            Vector3 emitterBoxDiffuseReflectivity = new Vector3(0.714f, 0.4284f, 0.18144f);
            GL.Uniform3(uDiffuseReflectivityLocation, emitterBoxDiffuseReflectivity);

            Vector3 emitterBoxSpecularReflectivity = new Vector3(0.393548f, 0.271906f, 0.166721f);
            GL.Uniform3(uSpecularReflectivityLocation, emitterBoxSpecularReflectivity);

            float emitterBoxShininess = 76.8f;
            GL.Uniform1(uShininessLocation, emitterBoxShininess);

            GL.BindVertexArray(mVAO_IDs[1]);
            GL.DrawElements(PrimitiveType.Triangles, mBoxModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);

            #endregion

            #region GridBox1


            Matrix4 m2 = mGridBox1Model * mWorld;
            uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref m1);

            Vector3 GridBox1AmbientReflectivity = new Vector3(0.2125f, 0.1275f, 0.054f);
            GL.Uniform3(uAmbientReflectivityLocation, GridBox1AmbientReflectivity);

            Vector3 GridBox1DiffuseReflectivity = new Vector3(0.714f, 0.4284f, 0.18144f);
            GL.Uniform3(uDiffuseReflectivityLocation, GridBox1DiffuseReflectivity);

            Vector3 GridBox1SpecularReflectivity = new Vector3(0.393548f, 0.271906f, 0.166721f);
            GL.Uniform3(uSpecularReflectivityLocation, GridBox1SpecularReflectivity);

            float GridBox1Shininess = 76.8f;
            GL.Uniform1(uShininessLocation, GridBox1Shininess);

            GL.BindVertexArray(mVAO_IDs[2]);
            GL.DrawElements(PrimitiveType.Triangles, mBoxModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);

            #endregion

            #region GridBox2


            Matrix4 m3 = mGridBox2Model * mWorld;
            uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref m1);

            Vector3 GridBox2AmbientReflectivity = new Vector3(0.2125f, 0.1275f, 0.054f);
            GL.Uniform3(uAmbientReflectivityLocation, GridBox2AmbientReflectivity);

            Vector3 GridBox2DiffuseReflectivity = new Vector3(0.714f, 0.4284f, 0.18144f);
            GL.Uniform3(uDiffuseReflectivityLocation, GridBox2DiffuseReflectivity);

            Vector3 GridBox2SpecularReflectivity = new Vector3(0.393548f, 0.271906f, 0.166721f);
            GL.Uniform3(uSpecularReflectivityLocation, GridBox2SpecularReflectivity);

            float GridBox2Shininess = 76.8f;
            GL.Uniform1(uShininessLocation, GridBox2Shininess);

            GL.BindVertexArray(mVAO_IDs[3]);
            GL.DrawElements(PrimitiveType.Triangles, mBoxModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);

            #endregion

            #region SphereOfDoomBox


            Matrix4 m4 = mSphereOfDoomBoxModel * mWorld;
            uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref m1);

            Vector3 SphereOfDoomBoxAmbientReflectivity = new Vector3(0.2125f, 0.1275f, 0.054f);
            GL.Uniform3(uAmbientReflectivityLocation, SphereOfDoomBoxAmbientReflectivity);

            Vector3 SphereOfDoomBoxDiffuseReflectivity = new Vector3(0.714f, 0.4284f, 0.18144f);
            GL.Uniform3(uDiffuseReflectivityLocation, SphereOfDoomBoxDiffuseReflectivity);

            Vector3 SphereOfDoomBoxSpecularReflectivity = new Vector3(0.393548f, 0.271906f, 0.166721f);
            GL.Uniform3(uSpecularReflectivityLocation, SphereOfDoomBoxSpecularReflectivity);

            float SphereOfDoomBoxShininess = 76.8f;
            GL.Uniform1(uShininessLocation, SphereOfDoomBoxShininess);

            GL.BindVertexArray(mVAO_IDs[4]);
            GL.DrawElements(PrimitiveType.Triangles, mBoxModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);

            #endregion

            this.SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            GL.BindVertexArray(0);

            GL.DeleteBuffers(mVBO_IDs.Length, mVBO_IDs);
            GL.DeleteVertexArrays(mVAO_IDs.Length, mVAO_IDs);

            mShader.Delete();

            base.OnUnload(e);
        }
    }
}
