# Procedural-Environment
Course project for Computer Graphics and Interaction

The goal of this project was to create procedural islands with surrounding water and clouds - all based on fractal noise (Simplex noise in this case).

The islands are created using three layers of noise, one noise can be set to "Combine" then acts like a filter between the two other 
noise layers. Typically one would create one layer of billow noise for the base terrain, one layer of ridged multifractal for mountains - and finally mix these with one layer of perlin noise. Each layer can be offseted indepedently, so the system is fully procedural but with a lot of artistic control over each layer.

The clouds and water are created with shaders using a Simplex noise implementation that runs on the GPU. Both clouds and water have several controls for real-time manipulating of the final look (denser clouds or bigger waves etc.).

Development is documented under CG16 tag on my blog: https://erikeriksson.wordpress.com/category/cgi16/

The project is playable in browser here: https://upscalebaby.itch.io/procedural-enviroments
