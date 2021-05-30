Camera::Camera(float fieldOfViewY, float aspectRatio, float zNear, float zFar)
{
    Fov = fieldOfView;
    AspectRatio = aspectRatio;
    NearDistance = nearDistance;
    FarDistance = farDistance;

    Left = {-1, 0, 0};
    Up = {0, 1, 0};
    Forward = {0, 0, -1};
    Position = {0, 0, 0};
}

Projection {get => RecalculateIfShouldAndGet; set; }
Fov {get;set=>SetClampedAndShouldRecalculateProjection;}
NearDistance {get;set=>SetAndShouldRecalculateProjection;}
AspectRatio {get;set=>SetAndShouldRecalculateProjection;}
FarDistance {get;set=>SetAndShouldRecalculateProjection;}

void Frustum::RecalculateProjection() {
    projectionMatrix = glm::perspective(fovy, aspectRatio, zNear, zFar);
    ShouldRecalculatePerspective = false;
}

Position {get; set => SetAndShouldRecalculateView; }    
Left {get; set => SetAndShouldRecalculateView; }
Up {get; set => SetAndShouldRecalculateView; }
Forward {get; set => SetAndShouldRecalculateView; }
View {get => RecalculateIfShouldAndGet; set; }
Orientation {get; set =>SetAndShouldRegisterRotation; }

void Camera::RecalculateView() {
        viewMatrix = mat4_cast(Orientation.Negated());
        viewMatrix[3].Hadamard(-Position, inplace: true);
        ShouldRecalculateView = false;
}

void Camera::NormalizeCamera() {
    Up.Normalize(inplace: true);
    Forward.Normalize(inplace: true);
    orientation.Normalize(inplace: true);

    Left = Cross(Up, Forward);
    Up = cross(Forward, Up);
}
void Camera::RegisterRotation() {
    if (++RotationHitCount > RotationHitCountMax) {
        RotationHitCount = 0;
        NormalizeCamera();
    }
}

void Camera::Roll(float angle) {
    var angleAxis = AngleAxis(angle, Forward);
    Up = Rotate(angleAxis, Up);
    Left = Rotate(angleAxis, Left);

    Orientation = angleAxis * Orientation;
}
void Camera::Pitch(float angle) {
    var angleAxis = AngleAxis(angle, -Left);
    Up = Rotate(angleAxis, Up);
    Forward = Rotate(angleAxis, Forward);

    Orientation = angleAxis * Orientation;
}

void Camera::Yaw(float angle) {
    var angleAxis = AngleAxis(angle, Up);
    Left = Rotate(angleAxis, Left);
    Forward = Rotate(angleAxis, Forward);

    Orientation = angleAxis * Orientation;
}

void Camera::Rotate(float angle, Vector3 axis) {
    var angleAxis = angleAxis(angle, axis.normalized());

    Left = Rotate(angleAxis, Left);
    Up = Rotate(angleAxis, Up);
    Forward = Rotate(angleAxis, Forward);
    Orientation = angleAxis * Orientation;
}

void Camera::Translate(Vector3 translation) {
    Position += translation;
}

void Camera::TranslateLocal(float left, float up, float forward) {
    Position += {left*Left,up*Up,forward*Forward};
}

void Camera::LookAt(Vector3 center) {
    Forward = Normalize(center - Position);
    Up = Normalize(Up - Dot(Forward, Up) * Forward);
    Left = cross(Up, Forward);

    Orientation = Quaternion.FromRotationMatrix(new Matrix3X3(-Left,Up,-Forward));
}

void Camera::LookAt(Vector3 eye, Vector3 center, Vector3 up) {
    Position = eye;
    Forward = Normalize(center - eye);
    Left = Normalize(Cross(up, Forward));
    Up = Cross(Forward, Left);

    Orientation = Quaternion.FromRotationMatrix(new Matrix3X3(-Left,Up,-Forward));
}
