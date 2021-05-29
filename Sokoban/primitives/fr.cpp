#include "Frustum.hpp"

#include <glm/gtc/matrix_transform.hpp>

namespace Rigid3D {

Frustum::Frustum(float fieldOfViewY, float aspectRatio, float zNear, float zFar)
        : fovy(fieldOfViewY),
          aspectRatio(aspectRatio),
          zNear(zNear),
          zFar(zFar),
          _isPerspective(true),
          recalcPerspectiveMatrix(false) {

    projectionMatrix = glm::perspective(fieldOfViewY, aspectRatio, zNear, zFar);
}

//----------------------------------------------------------------------------------------
mat4 Frustum::getProjectionMatrix() const {
    if (recalcPerspectiveMatrix) {
        projectionMatrix = glm::perspective(fovy, aspectRatio, zNear, zFar);
        recalcPerspectiveMatrix = false;
    }
    return projectionMatrix;
}

//----------------------------------------------------------------------------------------
float Frustum::getFieldOfViewY() const {
    return fovy;
}

//----------------------------------------------------------------------------------------
float Frustum::getAspectRatio() const {
    return aspectRatio;
}

//----------------------------------------------------------------------------------------
float Frustum::getNearZDistance() const {
    return zNear;
}

//----------------------------------------------------------------------------------------
float Frustum::getFarZDistance() const {
    return zFar;
}

//----------------------------------------------------------------------------------------
bool Frustum::isPerspective() const {
    return _isPerspective;
}

//----------------------------------------------------------------------------------------
bool Frustum::isOrthographic() const {
    return !_isPerspective;
}

//----------------------------------------------------------------------------------------
/**
 * Sets the \c Frustum field of view angle.
 *
 * @note If parameter \c fieldOfViewY is negative, the fieldOfViewY for the \c
 * Frustum is set to zero.
 *
 * @param fieldOfViewY
 */
void Frustum::setFieldOfViewY(float fieldOfViewY) {
    if (fieldOfViewY < 0.0f) {
        fieldOfViewY = 0.0f;
    } else if (fieldOfViewY > 180.0f) {
        fieldOfViewY = 180.0f;
    }
    this->fovy = fieldOfViewY;
    recalcPerspectiveMatrix = true;
}

//----------------------------------------------------------------------------------------
void Frustum::setAspectRatio(float aspectRatio) {
    this->aspectRatio = aspectRatio;
    recalcPerspectiveMatrix = true;
}

//----------------------------------------------------------------------------------------
void Frustum::setNearZDistance(float zNear) {
    this->zNear = zNear;
    recalcPerspectiveMatrix = true;
}

//----------------------------------------------------------------------------------------
void Frustum::setFarZDistance(float zFar) {
    this->zFar = zFar;
    recalcPerspectiveMatrix = true;
}

//----------------------------------------------------------------------------------------
void Frustum::setProjectionMatrix(const mat4 & projectionMatrix) {
    this->projectionMatrix = projectionMatrix;
}

} // end namespace Rigid3D