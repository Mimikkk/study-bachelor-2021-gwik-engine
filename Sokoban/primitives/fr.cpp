Frustum::Frustum(float fieldOfView, float aspectRatio, float nearDistance, float farDistance)
{
    Fov = fieldOfView;
    AspectRatio = aspectRatio;
    NearDistance = nearDistance;
    FarDistance = farDistance;
}

Projection {get => RecalculateIfShouldAndGet; set; }
Fov {get;set=>SetClampedAndShouldRecalculateProjection;}
NearDistance {get;set=>SetAndShouldRecalculateProjection;}
AspectRatio {get;set=>SetAndShouldRecalculateProjection;}
FarDistance {get;set=>SetAndShouldRecalculateProjection;}

mat4 Frustum::RecalculateProjection() {
    projectionMatrix = glm::perspective(fovy, aspectRatio, zNear, zFar);
    recalcPerspectiveMatrix = false;
}
